# <img align="left" alt="wc3proxy" src="./media/wc3proxy-gui.png" height="100" width="100"> WC3 Proxy
portable, cross-platform, [GUI](https://en.wikipedia.org/wiki/Graphical_user_interface) for the [`wc3proxy`](https://code.google.com/archive/p/wc3proxy/) utility.

<br clear="both">

play **Warcraft III** with your remote friends just like if you were all in the same **LAN**!

<br>

<img align="right" alt="mac" src="./media/windows%402x.png?x=4" height="100"/>
<h2><a href="/#">download for windows</a></h2>
<br clear="both">

<img align="right" alt="windows" src="./media/mac%402x.png?x=4" height="100"/>
<h2><a href="/#">download for mac</a></h2>
<br clear="both">

<img align="right" alt="linux" src="./media/linux%402x.png?x=4" height="100"/>
<h2><a href="/#">download for linux</a></h2>
<br clear="both">

## how to use
this is how **I had great success playing (pre-reforged versions)** across the atlantic:
1) have everyone in the same [`ZeroTier`](https://www.zerotier.com/) network.
2) choose one person to host and note their ZeroTier IP address.
3) all other players need to run [`WC3 Proxy`](https://github.com/leonardodino/wc3proxy) configured to the host IP.
4) everyone must use port `6113` on their game settings (`options` -> `gameplay` -> `game port`)

**troubleshooting**:
- the host should not run this utility (not sure if it impacts anything, but there's no need to)
- everyone should be on the same protocol version, and configure it on the proxy
- please use a stable connection, as there's no relay server between the nodes


## credits
- [original `wc3proxy`](https://github.com/FooleAU/wc3proxy) by @FooleAU
- [network code cleanup](https://github.com/evshiron/wc3proxy) by @evshiron
- [`dotnet core` port](https://github.com/marcsanfacon/wc3proxy) by @marcsanfacon
- [moonwell icon](https://www.artstation.com/artwork/YwED6) by [taylormouse](https://taylormouse.artstation.com/)
- [Warcraft III](https://en.wikipedia.org/wiki/Warcraft_III:_The_Frozen_Throne) by [blizzard](https://blizzard.com/)
- boredom caused by the `2020 Plague Season™`
