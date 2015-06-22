# WC3 Proxy

This is a repository exported from [wc3proxy - WC3Proxy allows you to play Warcraft 3 multiplayer over the internet without using Battle.Net. - Google Project Hosting](https://code.google.com/p/wc3proxy/).

I found it from the Internet and choose it because it is open source, but when I teach my classmates how to use it, I feel weird.

* It tries to do a Reverse DNS Query when an IP address is entered
  * And when it fails, fallback to localhost
* It saves the inputs to Registry, make it hard to clean
  * And if the record is wrong, leads to some other problems
* It should be able to find the executable from current directory
* It should have Unicode support

Thankfully, the code is not too heavy. I have done some commits to fix the problems above. Hope someone like it :)
