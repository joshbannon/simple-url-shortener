# Simple Example Link Shrink Library
This is an example library that uses the basic patterns described at the root of this project. 
It contains a class to PUT redirects to S3, and two different strategies for making a short route
from a long URL.

Variations are endless here. You could prepend custom routes but generate a random suffix. You
could wrap the RandomShrinkStrategy in a RetryStategy to detect taken links and generate another random
one instead, or you could wrap the HashingShrinkStrategy with a ReuseStrategyand just return a
matching link when it was already created.