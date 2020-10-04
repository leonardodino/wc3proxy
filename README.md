# <img align="left" alt="wc3proxy" src="./media/wc3proxy-gui.png" height="100" width="100"> WC3 Proxy
portable, cross-platform, [GUI](https://en.wikipedia.org/wiki/Graphical_user_interface) for the [`wc3proxy`](https://code.google.com/archive/p/wc3proxy/) utility.

<br clear="both">

play **Warcraft III** with your remote friends just like if you were all in the same **LAN**!

<br>

## [download][releases-page]
| [![windows](./media/windows%402x.png)][download-windows] | [![mac](./media/mac%402x.png)][download-mac] | [![linux](./media/linux%402x.png)][download-linux] |
|:---:|:---:|:---:|
| [**windows**][download-windows] | [**mac**][download-mac] | [**linux**][download-linux] |

<br>

## how to play
**requirements**:
1) everyone has the exact same game version (numbers on the bottom right corner of the menu).
2) all players must set port `6112` on their game settings (**options** → **gameplay** → **game port**).
3) download this utility and [`ZeroTier`](https://www.zerotier.com/).

this is how **I had great success playing** on pre-reforged versions:
1) have everyone in the same [`ZeroTier`](https://www.zerotier.com/) network.
2) choose one person to host and note their ZeroTier IP address.
3) all other players need to run [`WC3 Proxy`](https://github.com/leonardodino/wc3proxy) configured to the host IP.

**troubleshooting**:
- the host should not run this utility (not sure if it impacts anything, but there's no need to)
- everyone should be on the same protocol version, and configure it on the proxy
- please use a stable connection, as there's no relay server between the nodes

<br>

## credits
- [original `wc3proxy`](https://github.com/FooleAU/wc3proxy) by [@FooleAU](https://github.com/FooleAU)
- [network code cleanup](https://github.com/evshiron/wc3proxy) by [@evshiron](https://github.com/evshiron)
- [`dotnet core` port](https://github.com/marcsanfacon/wc3proxy) by [@marcsanfacon](https://github.com/marcsanfacon)
- [moonwell icon](https://www.artstation.com/artwork/YwED6) by [taylormouse](https://taylormouse.artstation.com/)
- [Warcraft III](https://en.wikipedia.org/wiki/Warcraft_III:_The_Frozen_Throne) by [blizzard](https://blizzard.com/)
- boredom caused by the `2020 Plague Season™`

[releases-page]: https://github.com/leonardodino/wc3proxy/releases
[download-windows]: https://github.com/leonardodino/wc3proxy/releases/latest/download/WC3Proxy.AppImage
[download-mac]: https://github.com/leonardodino/wc3proxy/releases/latest/download/wc3proxy.dmg
[download-linux]: https://github.com/leonardodino/wc3proxy/releases/latest/download/WC3Proxy.AppImage
