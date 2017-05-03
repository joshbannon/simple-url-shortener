# Setting Up the World's Lowest Maintenance URL Shortener

I've had to set up a URL shortener a few times now, and it's something I'd like to never have to think about again. 
My goal here is to document the simplest, cheapest and most maintenance free solution possible.

Briefly, here's the core of the system. Variations are possible and might make sense for your needs.

AWS Route 53 (Alias Records) -> AWS S3 Static Website <- URL Redirect Header <- My Application

This suggested setup fulfils most needs. Additional capabilities can easily be added on.

There's a few things going on. Here's a quick explanation since some people may not be familiar with all parts.

## URL Redirect Header

Fundamentally, a URL shortener like bitly, goo.gl or any of the others is just a web server that waits for a call on a 
short URL and returns a more descriptive URL as a redirect. There are other options, but for the general case we'll 
return a "301 redirect" which instructs the browser that if it sees the same small URL again it should just continue 
on to the longer route without reconfirming with the URL shortener service.

In Example:
>GET http://shor.tly/abc123 

>HTTP/1.1 301 Moved Permanently  
>Location: http://maps.google.com/  
>... 


## AWS S3 Static Website

S3 hits the sweet spot for a lot of simple server jobs for me. It's dead simple, bullet proof, extremely scalable, and 
it's really inexpensive for many use cases. Most people talk about S3 as a file store, but at its heart, S3 is a simple 
static web server that accepts GETs and PUTs. In our case, we'll be PUTing the desired redirect response, which our 
users will see when the browser GETs that resource later.

There are a couple more tricks in S3's bag that really makes it stellar for our purposes though.

### Expiration

Every item you place into S3 can be set to expire. If your short links are intended to be temporary, then you can say 
goodbye to ongoing maintenance. Just set an expiration on your S3 item and it will delete itself when you no longer 
want it.

### Logs

S3 is a well behaved web server, and it has the logs to prove it. If you need to, turn on S3 logs in order to track 
access.

### Potential Variation -- AWS CloudFront

Using S3 on its own is fine for many purposes, but you might want to layer CloudFront on top of it if you expect 
tremendous amounts of traffic to the same resource from all over the world. 

You should also consider whether you want to provide secure URLs for your shortener. The obvious answer would seem to 
be yes, and if so you'll need something like CloudFront to do SSL termination for you. But does this always make sense?
 SSL is almost always a good idea, but there are a few problems when it comes to a URL shortener. 

First, https is simply longer than http. You might have just spent a great deal of money to get a URL that is a single 
character shorter, do you want to add one more back in? Security wise, the value of SSL with a shortener is dubious at 
best, and it might promote false confidence. 

There's nothing to stop your user from requesting an insecure URL even if you specify a 
secure one; in fact if your use case involves the user typing the URL in on their phone, they will almost never 
preface with https. As soon as they've done that, the short URL is in the clear regardless of whether you'd provided a 
way to transmit securely. Because of this, you should think carefully before using any URL shortener for any purpose 
that requires significant security. If the final web page does need to be secure, and might contain any important 
private information, then it must require authentication and it must never include any revealing information in the 
full descriptive URL.

All that said, there's also nothing to stop your user from typing a secure url even if you provide an insecure one.
You can provide SSL capability but emphasise these security concerns and never try to use it while 
expecting the short URL or the exchanged descriptive URL to remain secret.

## Route 53

Route 53 is Amazon's DNS service, and it's a pretty critical part of this stack. You won't be able to use your existing DNS service because we need to take advantage of a special feature. You see, in order to resolve a bare domain (like shor.tly as opposed to www.shor.tly) you need an apex DNS record, and an apex record is only allowed to resolve to an actual concrete IP. This is a big problem because we don't have an IP to give. S3 IPs could change at any time. 

So what do we do? Well, AWS has provided a little bit of black magic here. Because they own the DNS server, and because they also own the S3 servers, they can perform a slight of hand. Route 53 has a custom record type called an alias record. When you define the alias record, you can point it at another domain (like shor.tly.s3.aws.com) and Route 53 will look up an appropriate IP to return on the fly.