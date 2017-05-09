# Simple Example Interface
This is an extremely simple interface as an example for using the LinkShrink library. No provision is made for security, reporting or any link management.

It can easily be run from Visual studio or the command line. Though you much first configure appsettings with your domain, bucket name, keys, etc...

Simple security for an internal app could be obtained simply by annotating the Entry Controller with a group name. If your application generates links for itself, as many do, you probably don't need even that much.