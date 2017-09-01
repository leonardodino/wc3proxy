# WC3 Proxy

This is a repository exported from [wc3proxy](https://github.com/evshiron/wc3proxy).

I modified the project to have it as a dotnet core console application to be able to run it on Mac/MacOS/OS X. It should also run on Linux and Windows.

To compile, make sure [dotnet core is installed](https://www.microsoft.com/net/core) and simply run *dotnet build* from the main directory.

To execute the proxy, simply run *dotnet run [hostname|ipaddress]*, where hostname|ipaddress is the machine hosting the Warcraft III - The Frozen Throne game. I suggest you also add --no-build to speed up starting the process. So that makes: *dotnet run --no-build nameoripofmachinehostingthegame*

*Note:*
I hardcoded the version 1.28 and The Frozen Throne, so by default, it does not work for Warcraft RoC or other version. This can be easily changed in MainProxy.cs.
