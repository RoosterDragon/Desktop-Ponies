# Desktop Ponies

8 bit ponies on your desktop.

![Desktop Ponies](https://i.imgur.com/7Z25ikB.png)
*[Desktop Background by CuteSkitty](https://cuteskitty.deviantart.com/art/Celestia-256362644)*

Desktop Ponies lets you choose some ponies from [My Little Pony: Friendship is Magic](https://wikipedia.org/wiki/My_Little_Pony:_Friendship_Is_Magic) to trot around your desktop whilst you work.

The ponies will move around on screen performing a variety of custom animations. They also talk occasionally and some ponies will interact with each other.

There are over 300 ponies and other characters from the show included. There is also a wide array of OCs available and you can create your own ponies too.

Don't want to restrict yourself to just ponies? Try downloading the [Equestria Girls](https://sta.sh/2ouk6qgs0sg) characters made by [Bot-chan](https://botchan-mlp.deviantart.com/).

## Installation

Desktop Ponies works on Windows, Mac and Linux.

* [Download the latest version (v1.67)](https://github.com/RoosterDragon/Desktop-Ponies/releases/download/v1.67/Desktop.Ponies.v1.67.zip)
* Extract the files.
* Check the included readme file for further instructions.

## License

The artwork is licensed under [Creative Commons Attribution-NonCommercial-ShareAlike 3.0 Unported (CC BY-NC-SA 3.0)](https://creativecommons.org/licenses/by-nc-sa/3.0/). This means you are allowed to share and alter the artwork, provided you give credit, do not use it for commercial purposes and release it under this same license. You can find assets for the ponies and other characters in the [Content/Ponies](Content/Ponies) directory. There is a [list of credits](Content/credits.txt) in the [Content](Content) directory.

The source code is available under the same license.

## External Links

* [Ponychan Mane Thread](https://www.ponychan.net/fan/res/458.html) - Discussion of the program and canon artwork.
* [deviantART Group](https://desktop-pony-team.deviantart.com/) - Group maintained by contributing artists to showcase their work.

## Documentation

Some information about the file formats employed by the program can be found in the [technical documentation](techdoc.md).

## Related Projects

* [DPengine](https://github.com/hidefromkgb/DPengine) - Multi-platform reimplementation of Desktop Ponies. Worth trying if you use Mac or Linux or need better performance.
* [Browser Ponies](https://panzi.github.io/Browser-Ponies/) - Desktop Ponies in Javascript. Get ponies in your browser.
* [New Browser Ponies](https://github.com/JasminDreasond/New-Browser-Ponies) - Fork from the Original Repository Browser Ponies updated with the last current new ponies and Cloudflare CDN support.
* [BronyLiveWallpaper](https://github.com/frankkienl/BronyLiveWallpaper) - Live Android Wallpaper using Desktop Ponies sprites.
* [PonyPaper](https://github.com/Smithers888/PonyPaper) - Live Android Wallpaper using Desktop Ponies sprites.
* [ponysay](https://github.com/erkin/ponysay) - cowsay reimplementation for ponies, using some Desktop Ponies sprites.
* [Linux/GTK+](https://github.com/killerrabbit/Ponies-for-Linux-GTK-) - Desktop Ponies reimplementation for Linux.
* [Desktop Ponies for OS X](https://github.com/xobs/desktop-ponies-mac) - Desktop Ponies reimplementation for OS X.
* [qponies](https://github.com/svenstaro/qponies) - Desktop Ponies reimplementation with Qt.
* [Qt-ponies](https://github.com/myszha/qt-ponies) - Desktop Ponies reimplementation with Qt.

## Building

This project evolved from a Windows only solution and whilst it just about runs on non-Windows machines thanks to Mono, it is not really portable in terms on building on other platforms (sorry about that).

You will need to install [Mono](https://www.mono-project.com/download/) in order to acquire the libraries for the Gtk/Cairo portion of the program that runs on non-Windows platforms. You will probably need to update the references for those dlls in each of the projects that requires them.

The Microsoft.DirectX.AudioVideoPlayback library is long since obsolete but is used to play audio. You need to install the [DirectX 9 redistributable](https://www.microsoft.com/en-us/download/details.aspx?id=35) in order to resolve this reference.

Once resolved, you have a standard Visual Studio solution split into three projects. Desktop Sprites is the library that handles rendering, Desktop Ponies is the pony specific part of the application and Release Tool is used to run image optimizers and package new releases.

If you want to use the Release Tool to optimize images, you will need to acquire the [gifsicle](http://www.lcdf.org/gifsicle/) and [pngout](http://advsys.net/ken/utils.htm) and drop them into the application directory for them to work.
