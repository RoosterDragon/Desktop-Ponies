﻿Desktop Ponies!
Have little ponies running around your desktop aimlessly! Might entertain you for a few minutes!

There is a thread maintained on Ponychan for the main program. 
Latest thread at time of writing:
Mane - https://www.ponychan.net/fan/res/458.html

If you need any help setting up or running the program, please post in the main thread.

The sprite artists maintain a community for their works on DeviantArt:
https://desktop-pony-team.deviantart.com/

---------------------------------------

Installation

* Windows
    You will need .NET framework 4.8 installed. This comes by default with newer versions of Windows,
    but you can also get it here: https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net48-web-installer
    Once installed, simply click on 'Desktop Ponies.exe' to start the program.
* Unix
    Running the Windows version under Wine is recommend for best performance.
    Alternatively, you can run via mono:
    You will need the latest stable version of mono. You can also download it from here: 
    https://www.mono-project.com/download/stable/
    Alternatively you can install the prerequisite packages for your platform. You will need packages for the mono runtime, visual basic, and gtk 2.
    These vary by distro, but here's a couple of known invocations:
        - Debian or Debian based (e.g. Ubuntu): apt-get install libmono-system-xml-linq4.0-cil libmono-microsoft-visualbasic10.0-cil libgtk2.0-cil
        - Arch Linux: pacman -S mono mono-basic gtk-sharp-2
    Once installed, you can start the program from the command line with "mono 'Desktop Ponies.exe'".
    It is recommended to run "mozroots --import --ask-remove" from the command line to install default Mozilla web certificates,
    which will allow the program to check online for updates.
* Mac
    You will need mono version 3.2.1 or better.
    Choose the "Mono MRE Installer" from: https://www.mono-project.com/download/stable/
    Once installed, you can run the "RunOnMac.command" file to start the program.

---------------------------------------

Patches

    If you have downloaded a patch version, you should apply it by copying the patch files over your existing version of Desktop Ponies.
    Overwrite files where prompted. Please be aware some patches may modify core ini files. If you have manually modified these files,
    you should merge the changes manually or else your modifications will be lost!

    If you have downloaded a patch but don't have a version of Desktop Ponies already, go back and download the full version!

---------------------------------------

Screensaver (Windows Only)

    The 'Desktop Ponies.scr' is a Windows screensaver file. You can preview it by double-clicking the icon.
    To install it as a Windows screensaver, right-click and choose "Install" from the context menu.
    If you create a profile named "screensaver", these characters will appear when the screensaver starts, otherwise you will get a random pony.

    The standard Windows command line arguments for screensavers can be used:
        * "Desktop Ponies.scr" /s
            Start in screensaver mode. This uses the 'screensaver' profile.
        * "Desktop Ponies.scr" /c
            Configure screensaver mode.
        * "Desktop Ponies.scr" /p
            Preview screensaver mode. This does nothing.

---------------------------------------

Autostart

    Starting the program from the command line using '"Desktop Ponies.exe" autostart' will start and show ponies straight away,
    using the 'autostart' profile. If you have not yet configured this profile, you will get a prompt.
    You can use this to quickly launch the program with your favourite ponies.

---------------------------------------

Change Log

Latest changes:

v1.69:
- Drag and sleep behaviors will now play thier speech when a pony encounters the drag or sleep states.
- Prevent a crash when running on a device without an audio playback device.

v1.68:
- Added Cozy Glow by rosie-eclairs.
- Switched to using NAudio instead of DirectX for sounds (Windows only).

v1.67:
- Fixed Yona folder nesting.
- Prevent saving a profile with an invalid name.

v1.66:
- Added Autumn Blaze by Bot-chan.
- Added Bright Mac by Bot-chan.
- Added Gallus by Bot-chan.
- Added Luster Dawn by Bot-chan.
- Update Ocellus by Bot-chan.
- Added Potion Nova by Bot-chan.
- Added Prince Pharynx by Bot-chan.
- Added Sandbar by Bot-chan.
- Added Silverstream by Bot-chan.
- Added Somnambula by Bot-chan.
- Added Windy Whistles by Bot-chan.
- Added Yona by Bot-chan.
- Added the ability to specify a background color (Windows only).
- Fix pony spawn locations when using multiple monitors.
- Fix crash involving InvalidAsynchronousStateException.

v1.65:
- Added Ocellus by Bot-chan.

v1.64:
- Added Pear Butter by emeralddarkness.
- Added Jasmine Leaf by Blaze5565.
- Added Bow Hothoof by Bot-chan.
- Added Smolder by Bot-chan.
- Added Sphinx by Bot-chan
- Fixed naming of some ponies.
- Improve launching on Mac.

v1.63:
- Added Songbird Serenade by Bot-chan.
- Added Tempest Shadow by Bot-chan.
- Added Tea Shop Pony by Blaze5565.
- Fixed launching on Mac.

v1.62:
- Added Countess Coloratura by Blaze5565.
- Added King Thorax by Bot-chan.
- Tag pets as non-ponies.
- Fix ponies thinking they were in the exclusion area when they were not.
- Fix crashes when setting up an exclusion zone or multiple monitors.

v1.61:
- Added "level 2" Changelings by Bot-chan.
- Readded Starlight Glimmer (Season 5) by Bot-chan.
- Added the ability to delete ponies from the editor.
- Fix the .zip file not extracting properly on non-Windows systems.
- Fix an issues that may cause images not to load and instead hang the program.
- Fix crash if the pony editor if you tried to pause with no pony selected.

v1.60:
- Rename Sue Pie to Cloudy Quartz.
- Added Sunburst and updated Starlight Glimmer by Bot-chan.
- Added a crystallized Princess Twilight Sparkle by Bot-chan.
- Added Flurry Heart by Bot-chan.
- Added Police Pony by Bot-chan.
- Added Princess Ember by Bot-chan.
- Added Nurse Tenderheat and Nursery Rhyme by Bot-chan.
- Added Plaid Stripes by Bot-chan.
- Added Saffron Masala and Coriander Cumin by Bot-chan.
- Added Mr Shy, Mrs Shy and Zephyr Breeze by Bot-chan.
- Added Fresh Coat by Bot-chan.
- Added hug interaction between Moonlight Raven and Sunshine Smiles by Bot-chan.
- Add popcorn behaviour to Starlight Glimmer by Bot-chan.
- Update Nurse Redheart by Bot-chan.
- Fix a category for Seabreeze.
- Fix crash when an invalid number is pasted into the pony count field.

v1.59:
- Added Limestone Pie and Marble Pie by Bot-chan.
- Added Lily Longsocks and Svengallop by Anonycat.
- Added Sunshine Smiles, Moonlight Raven and Nerdy Delegate by Bot-chan.
- Added Granny Smith (Teenager), Sourpuss and Spoiled Rich by Bot-chan.
- Added Sailor Moon ponies by Bot-chan.
- Updated Vinyl Scratch by Bot-chan.
- Interactions are now saved per-pony, rather than in their own file.
  If you have an interactions.ini file, it will be upgraded automatically when the program starts.
- Added the ability to filter ponies by excluding certain tags.
- Fix category for Zipporwhill's Father.
- Fix pony editor to not mess with sort order of tables when saving.
- Fix some columns not being sortable in the pony editor.
- Fix a crash that would occur if you save a config for multiple monitors, then load the program with less monitors available.
- When using autostart, fix the main menu not working when returning to it.
- When using autostart, the program will minimize during loading.

v1.58:
- Added Greta, Grampa Gruff, Whoa Nelly and Stormy Flare by Bot-chan.
- Corrected Leadwing's name.

v1.57:
- Added Gustysnows, Moondancer and Quiet Gestures by Bot-chan.
- Updated Royal Guard by Bot-chan.
- Renamed Colgate to Minuette.
- Increase the default max pony count to 500.
- Fixed an issue where ponies in a profile that do not exist count towards the total pony count.
- Fixed ponies not moving in properly random directions.
- Fix a rare crash when trying to return to the menu.
- Improve some error messages.

v1.56:
- Added Gizmo by Bot-chan.
- Added Biff, Rogue and Withers by Bot-chan.
- Added Dumb-Bell, Hoops and Score by Bot-chan.
- Added Night Light by Bot-chan.
- Added Parcel Post, Smooze and Tree Hugger by Bot-chan.
- Added Cranky Doodle Donkey and Matilda by Bot-chan.
- Added Button Mash by Bot-chan.
- Added comic ponies by Bot-chan.
- Updated Lebowski ponies by Bot-chan.
- Added galloping Princess Twilight Sparkle animation by Bot-chan.
- Updated Changeling by Bot-chan.
- Fixes to Soigne Folio by Bot-chan.
- Fixed Maud Pie sprite from Bot-chan.
- Fix Archer's gender.
- Prevent a crash if the Profiles folder is deleted.
- Prevent a crash when loading a pony in the editor if you change selections quickly.

v1.55:
- Added Maud Pie by Bot-chan.
- Added Trouble Shoes by Bot-chan.
- Added Rainbow Blaze by Bot-chan.
- Added Toe-Tapper and Torch Song by Bot-chan.
- Added Adagio Dazzle, Aria Blaze and Sonata Dusk by Bot-chan.
- Updated Big McIntosh, including ponytones outfit, by Bot-chan.
- Added Zipporwhill and Zipporwhill's Father by Bot-chan.
- Added Written Script by Bot-chan, including a nuzzle interaction with Carrot Top.
- Added Cloakseller by Pony Noia, including an interaction with Rarity.

v1.54:
- Added Starlight Glimmer, Double Diamond, Night Glider, Party Favor and Sugar Belle by Bot-chan.
- Added Maud Pie by Anonycat and Bot-chan.
- Added Daring Do gallop animation by Bot-chan.
- Updated Changeling by Bot-chan.

v1.53:
- Added Laid-back Mule, Silver Shill, Mane-iac henchponies and Doctor Horse by Bot-chan.
- Updated Sunset Shimmer and added dance animations for Pinkie Pie and Princess Cadance by Bot-chan.

v1.52:
- Added Neon Lights by Bot-chan.
- Added Ponyville Library by StarStep.
- When editing a pony, it will be respawned when changes are made to ensure it gets the updated configuration.
- Added some scrollbars to the window for setting up transparency for GIF images.
- Prevent a crash when the current profile selection cannot be remembered due to file security.

v1.51:
- Added Flutterbat, Aunt Orange, Goldie Delicious, Tealove, Truffle Shuffle and Mane 6 Breezies by Bot-chan.
- Added Surprise change, Applejack banjo and drag, Lyra lyre, Pinkie Pie sleep, Saddle Rager transform,
  Rainbow Dash swim and Bulk Biceps drag, muscles and fly animations by Bot-chan.
- Updated Nightmare Moon and new fly animation by Bot-chan.
- Updated Rarity gala animations by The Coop.
- Added Filly Rarity Smile animation by RJP!
- Tweaked Princess Luna animations by Bot-chan.
- Fix tab key in the menu to go through ponies in order.
- Fix minor bug where ponies would appear in a random order if the Random Pony was missing.
- The Random Pony is now ignored when using the keyboard to jump to a pony by name in the menu.

v1.50:
- Added A.K. Yearling, Featherweight, Lady Justice, Stellar Eclipse and Tirek by Bot-chan.
- Added crystal Applejack, Fluttershy, Pinkpie Pie and Rarity by Bot-chan.
- Revamped Parasprite, Bulk Biceps by Bot-chan.
- Added Rarity dragging animations, Cheery Berry with goggles and tweaks to Spitfire's goggles by Bot-chan.
- Added balloon riding animations for Cherry Berry and Twilight Sparkle by Bot-chan.
- Added bench interaction for Lyra, Bon-Bon and Shoeshine by Bot-chan.
- Added Mr Breezy and Mr Greenhooves by Anonycat.
- Renamed Master to Perfect Pace, Clyde Pie to Igneous Rock and added revamped animations by Anonycat.
- Revamped Braeburn, Davenport, Hayseed Turnip Truck and Horte Cuisine by Anonycat.
- Added Carrot Top raising 'Welcome Princess Celest' banner as an interaction with Twilight Sparkle by StarStep and Anonycat.
- Fixed the "Do not repeat animations" setting to work again for behaviors and effects.
- Fixed a minor issue where a pony resuming its old behavior after exiting sleep, mouseover or drag would not re-trigger effects for that behavior.
- Fixed creating/editing of interactions to keep the list of possible behaviors updated as you add or remove targets.
- Fixed renaming behaviors and speeches in the editor to update places the reference those names too.
- Prevent a crash that would occur if you deleted the random pony, introduced in 1.47.
- Fixed a crash that could occur when changing the screen resolution with ponies active on Windows.
- Fixed a crash that could occur when closing the program on Windows.

v1.49:
- Added Carousel Boutique and The Moon by StarStep.
- Added Farmer Rarity, Twilight Sparkle's Starswirl costume, new AJ galloping and rearing animations, CMC galloping animations, an extra Queen Chrysalis walk
  and some sax transitions for Blues by Bot-chan.
- Added a make-it-stop animation for Spot by Pony Noia.
- The 0/1 of All Ponies buttons now apply to only those ponies in the current filter, and also exclude the special random pony.
- When an interaction is considering for starting, ponies will now individually choose a suitable behavior to run, subject to group restrictions. This means:
  - Interactions in 'Any' mode may be able to run more often, as targets will be ignored individually if they lack a suitable behavior at the time.
  - Interactions with multiple behaviors can now be engineered to allow targets to start different behaviors depending on their type and current group.
- Diagonal movement will now take place on a randomly placed axis between 15 and 75 degrees rather than at exactly 45 degrees.
- When diagonal/vertical or diagonal/horizontal movement is used, if diagonal movement is used the diagonal axis is placed randomly
  between 15 and 45 degrees from the other axis in the combination.
- Ponies can now be dragged again whilst asleep.
- Fixed some rare crashes that could occur when returning to the menu or closing the program on Windows.

v1.48
- Improved how behaviors with small images act when near the screen boundary.
- Improved how ponies are deployed from and recalled to houses.
- Fix an issue from 1.47 that would cause a pony to wrongly use a mirrored left image when facing right rather than the actual right image.

v1.47:
- Added new and updated Doctor Whooves animations by Anonycat.
- Added Seabreeze by Bot-chan.
- Added new animations for Fleetfoot, Soarin', Spitfire and Surprise by Bot-chan.
- Updated animations for Granny Smith by Bot-chan.
- Added Lily panic animations and an updated Mrs Sparkle idle animation by Bot-chan.
- Setting up the screensaver has been made easier.
- Creating a new pony in the editor has been streamlined. The new pony will appear without having to reopen the editor.
- The dialogs for creating new behaviors, effects, speeches and interactions in the editor have been improved.
- Improved how ponies with custom image centers react when at the screen boundary.
- Fixed not being able to take control outside of games.
- Fixed a bug where closing the Desktop Ponies window would not return you to the main menu.
- Reduced memory usage on Windows.

v1.46:
- Added Fili-Second gallop animation by Bot-chan.
- Fixed a bug in the editor that was preventing renaming of behaviors, effects and speeches.
- Improved how ponies react when at the screen boundary.
- Fixed initial effect sometimes deploying in the incorrect location.

v1.45:
- Added Slendermane and Trenderhoof by Bot-chan.
- Improve several aspects of how ponies behave.
- When returning to the menu, any sounds playing will stop immediately.
- Fix editor not being able to create a new pony since 1.44.
- Fix pony editor being hidden when a pony is first selected since 1.44.
- Fix a crash from 1.44 that would occur when games ended.

v1.44:
- Added Nurse Snowheart, Nurse Sweetheart, Fleetfoot, Cheese Sandwich and the Power Ponies by Bot-chan.
- Added Ms Harshwhinny by RoboKitty.
- Renamed Horse Power to Bulk Biceps.
- Renamed Pinkamina Diane Pie to Pinkamena Diane Pie.
- Renamed Princess Cadence to Princess Cadance.
- Renamed Shining Armour to Shining Armor.
- Renamed Big Macintosh to Big McIntosh.
- You can now set the area ponies appear by manually entering an area, if you want finer control than a per-monitor selection.
- Setting up image centers has been made easier, you can type in the values and mirror them between the left and right images.
- The options screen has been tidied up, and a few minor issues with options have been fixed.
- Interactions are now also restricted by behavior groups.
- Interupting a pony during an interaction they are taking part in will cancel the interaction amoung those involved.
- A new attribute has been added for behaviors - "Image Offset Type". If a behavior specifies a pony to follow, you can specify
  if the offset should be mirrored when the target switches direction by choosing the "Mirror" setting. The default setting of
  "Fixed" does not change the offset and matches how Desktop Ponies has treated the offset in the past.
- Fixed a problem where attempting to add new ponies via the context menu on Linux/Mac would sometimes not work.
- Fixed a crash that would occur very rarely when returning to the menu or closing the program.

v1.43:
- Added Ahuizotl, Mane-iac, Suri Polomare, Scootaloo fly animation, Spitfire's cutie mark,
  animated idle image for Candy Mane and Dinky Hooves and an animated TARDIS by Bot-chan.
- Added Apple Split, Babs Seed, Hayseed Turnip Truck and Jesús Pezuña by Anonycat.
- Added Coco Pommel by StarStep.
- Added Lightning Dust by Starly and Bot-chan.
- Added Grace Manewitz by Pony Noia.
- Added Daring Do fly animation by Anonymous and StarStep.
- Tweaks to Bon-Bon's color scheme and idle animation by Bot-chan.
- Added a custom menu for Mac - to resolve issues where it was not possible to start ponies on this platform.
- Fixed a divide by zero error on Windows.

v1.42.5:
- Added Princess Twilight Sparkle starburst animation by Bot-chan.
- Changed a couple of pony configs to utilise some images that weren't actually being used.
- Fix a regression where the transparent regions of effects sometimes allowed them to be dragged, even if a pony should have been preferred.
- Memory usage on Windows has been reduced significantly.
- Rendering speed on Windows has been improved for large numbers of sprites, and when a scale factor is set.

v1.42.4:
- Change most of Fluttershy's speeches to be in group 0, so she will talk more often.
- Rendering speed on Windows has been significantly improved, allowing more ponies for less CPU.
- Fix a bug in the editor preventing creating or editing some parts of interactions.
- Fix a bug on Windows where transparent regions of a pony where sometimes clickable.

v1.42.3.2:
- Added "party hard" Twilight animation by ponynoia.
- Fixed some ponies getting stuck in the doorway when returning to houses.
- Fixed reloading options sometimes causing a crash if ponies were running.
- Fix an error where unrecognised tags against a pony were removed rather than ignored if you edited the tags.
- If a behavior has a min duration higher than the max duration, the values will be silently swapped around to remain sensible.
- Fixed a bug where old style speech definitions in .ini files were being ignored.
- Fixed a few things in the pony editor so they could handle ponies without any behaviors.

v1.42.3.1:
- Fixed interactions not respecting reactivation delay.
- Prevented crash when interacting with targets lacking the required behaviors.

v1.42.3:
- Added Ms. Peachbottom by Loaded--Dice.
- Added Pound Cake, Pumpkin Cake, Shopkeeper & Shoeshine by Bot-chan.
- Added a Community Links screen. This lists some useful community URLs and will also notify you when new releases become available.
- Ponies with missing image files will no longer show up for selection, but will show up in the editor so the links can be fixed.
- Prevent mini-games crashing when using ponies with no 'Any' group behaviors.
- Fixed effects that were not following their parent ponies when they were configured to do so.
- Fixed interactions not being considered for use if more ponies than just the interaction targets were present.
- Fixed crash when trying to start ponies on Windows and the profile specified alpha blending to be off.
- Fixed crash when navigating to previous behaviors in the image centers editor screen.

v1.42.2.9:
- Added Flash Sentry by Bot-chan.
- Added an interface for specifying translucent colors for GIF images to the editor. 
- Ponies may no longer follow effects. (This stops the program getting confused between following a pony and effect with the same name.)
- On Windows, running with alpha blending enabled is now just as fast as with it disabled.
- When changing linked behaviors in the editor, the "link order" column will now update as changes are made.
- The editor now handles config files that use different casing when it is allowed, rather than giving warnings.
- Prevented addition/removal/sleeping of ponies during games.
- Interaction target names are now case sensitive - for consistency.
- Fixed some issues related to editing interactions.
- Fixed bug preventing creation of new profiles.
- Fixed crash that happened when certain ponies were deployed from houses.
- Fixed a crash that occurred when editing a house that was also deploying a new pony.
- Fixed error that could cause Mac/Unix systems to not load the program fully.
- The program is now much more robust in the face of malformed .ini files and missing images.

v1.42.2.8:
- Fixed bug from 1.42.2.5 that prevented saving ponies if the program was not running on the same drive as the temp folder.

v1.42.2.7:
- Added Sunset Shimmer by Bot-chan.
- Fixed bug from 1.42.2.6 that prevented creating new ponies in the editor.

v1.42.2.6:
- Fixed destination offsets for behaviors. These work again.
- Game selection interface redesigned.
- Pony editor should be slightly snappier when switching between ponies, and on initial load.
- Pony editor will now prevent some invalid characters in names.
- Added some Windows-only options for showing the ponies in the taskbar, and a performance graph.
- Added option for showing pony logs. These might prove useful to people designing behaviors and interactions for seeing what the pony is doing.
- Error handling improved.

v1.42.2.5:
- Added Prince Blueblood by Bot-chan.
- Fixed some dodgy loading of interactions.
- Fixed Fluttershy's config which was preventing random behavior selection working effectively.

v1.42.2.4:
- Added Fiddlesticks, Ginger Snap, Manticore by Bot-chan.
- Updated flight animations for Princesses Celestia, Luna, Cadence & Queen Chrysalis by Bot-chan.
- Fixed bug in v1.42.2.3 where interactions would not be loaded by the pony editor.

v1.42.2.3:
- Fixed screensaver mode when using a background color or image.
- UI is now more responsive whilst loading.

v1.42.2.2:
- Fixed manual control outside of games.
- Fixed an error that could occur when the "Ponies attempt to avoid other windows" option was active.
- Fixed error that was crashing the pony editor since 1.42.2.1

v1.42.2.1:
- Fixed bug in v1.42.2 where ponies would freak out near screen edges.
- Fixed manual control during games.
- Fixed bug where removing all ponies did not always return you to the main menu.

v1.42.2:
- Games are now working again.
- If a pony is removed, any effects it spawned are now removed immediately.
- Fixed a bug using multiple monitors where ponies would bounce off all edges instead of moving across screens.
- Fixed a bug from v1.42.1.3 that could occasionally crash the program on startup.

v1.42.1.3:
- Added updated Changeling by Bot-chan
- Added Princess Twilight Sparkle by Bot-chan
- Changed config files for Spitfire and Trixie to allow them to switch between behavior groups (suggested by Anon).
- Changed random speeches so those in group 0 may play regardless of the current group.
- Fixed sounds not working in v1.42.1.2.
- Hopefully the final fix for ponies and idle animations during interactions.
- Fixed an error in the editor where the dialog to change an interaction target would fail to open.
- Loading times significantly improved.

v1.42.1.2:
- Added crystallized Twilight Sparkle by supersaiyanmikito
- Add Screw Loose by Bot-chan
- Much more solid fix for ponies not using the idle animations during interactions when they stop moving.
- Initial loading time should be a few seconds faster.

v1.42.1.1:
- Added Ace & Big Lebowski ponies by Bot-chan
- Added crystallized Rainbow Dash by supersaiyanmikito
- Fixed not being able to use keyboard to jump to ponies on the main menu after making changes in the editor.

v1.42.1:
- Added Owlowiscious by Bot-chan
- Ponies should be better at using their idle animations at the right time when in interactions.
- Fixed an error that would occur if you changed sound volume whilst a sound was playing.

v1.42.0.9:
- Really fixed an error about DirectX.
- Fixed a minor issue under Windows where using multiple monitors would create an oversized drawing area.

v1.42.0.8:
- Fixed an error about DirectX.

v1.42.0.7:
- Fixed a KeyNotFoundException error that could occur on startup.
- Another speculative fix for Mac UI issues.

v1.42.0.6:
- Fixed a problem on Windows if you had multiple monitors and the primary was not the top-leftmost monitor.
- Forcibly refreshed some items on the main menu UI when using pagination on the Mac to try and workaround mono issues.

v1.42.0.5:
- Fixed images for several ponies which had zero duration frames (and thus those frames would not be shown).
- Fixed a couple of images with transparent final frames that had been optimized away by accident.
- More speculative fixes for scrolling problems on the Mac.
- Fixed a bug where the pony control panel on Mac would add the wrong pony from the list.
- Fixed wonky scrollbar when restoring the window from the taskbar.
- Fixed a problem where saving interactions would use the lowercase name of the pony, thus causing errors.
- If the program encounters an error, it will attempt to log this to "error.txt" for later reference.

v1.42.0.4:
- Added pagination to the main menu. This is primarily to provide a workaround to the scrolling issues on Mac.
- Fixed long standing bug where toggling a tag in the filter on the main menu would not refresh the display straight away.
- Fixed a problem with copying images/sounds for new behaviors/effects under mono.
- Fixed an issue with deleting rows from the editor not having an effect when the file was saved.

v1.42.0.3:
- Added King Sombra by Bot-chan
- Fixed editor attempting to create new sound files and images in the wrong directory.
- Fixed exclusion zone preview in options menu.
- Changed paths will now show straight away in pony editor.

v1.42.0.2:
- Fixed screensaver issues when actually started by Windows.
- Fixed an issue trying to create a new pony in the editor without selecting another pony first.
- Possibly fixed an issue trying to load the default profile on Mac.

v1.42.0.1:
- Fixed some screensaver issues.
- Changed the main menu design in hopes of fixing some scrolling issues on the Mac platform.

v1.42:
- Added lots of new ponies from various artists.
- Support for Mac/Unix systems.
- Support for alpha blending.
- Some UI improvements.
- Slew of bug fixes.


V1.42 TEST 2

(test 1)
-Options for Minimum and Maximum # of spawn ponies added to houses.  (Note that these are per house TYPE, not per house spawned).
-Bias option for houses:  Is the random number generator giving you not enough ponies?  Too many?  Adjust the slider on the house option page!
-The "ID" object for a pony wasn't working.  Fixed.  This should prevent flickering when two ponies are on the same spot and are of the same height (they were sorting the same).
(test 2)
-You can now save multiple profiles.
-Some profile names are special:
   If you name a profile "autostart" then it will be loaded when you use the autostart option.
   If you name a profile "screensaver" then it will load when the screensaver is used.
You can also use the command line:  "autostart profilename" to load the 'profilename' profile.
-The save button on the main screen now saves all settings, instead of just the # of ponies.


V1.41 (Release - includes test version changes below)

-Applejack will put on her NMN costume less often.
-Pinkie pie will put on her silly mask less often (it was set to 70% instead of 7%?)
-Mysterious Mare-do-Well had some invalid behavior options that caused a crash sometimes.  Fixed.
-Quitting a pony that is involved in an interaction will now cancel the interaction.
-Occasionally ponies wouldn't enter a house and got stuck in a loop.  Fixed.
-The cancel button on the house options menu works now.
-Flickering when a pony reached their destination has hopefully been reduced.
-A rare crash caused by trying to draw() a negative frame # has been fixed.


V1.41 TEST 3

-Simply minimizing the main menu will no longer cause a crash.
-Behaviors and Speeches have a new setting:  "group".  Using this you can combine a set of behaviors into different groups, so that ponies can change state.  
For example:  Fluttershy and Gala Fluttershy have been combined.  Fluttershy will go into "gala" mode only if she runs her "goto_gala" behavior, and will stay in Gala mode only running behaviors in that group
until she runs her "leave_gala" behavior, which links back to the normal group.  These transition behaviors are simply linked (or "chained") behaviors.  If you wanted to put a transition animation in, you would do to in these two behaviors.
A pony may also change states if they leave a house.  Note that group 0 is a wild card - behaviors in this group will play regardless of what state the pony is in.
-The editor will no longer refresh and reprocess items every time you make a change.  To ensure that all data is valid and to regenerate the list of chained behaviors and other items, click the new "Apply/Refresh/Validate" button.
-The editor will now warn you if the pony you are working with has duplicate names, effects, speeches, or interactions.

Pony changes/updates:
-Dr. Whoof has had categories applied.
-Fluttershy has been combined with Gala fluttershy - see "groups" as mentioned above.
-Fluttershy's behaviors have been cleaned up, notably the angel follow behaviors which were malfunctioning.
-Cape-less Trixie has been combined with normal Trixie.  Also an example of behavior grouping.
-Suit-less Spitfire has been combined with Normal Spitfire.
-Big Celestia's images have been fixed thanks to RoosterDragon; she can walk again!
-Conga line interaction between the mane 6 - Bot-chan
-Pinkie Pie updates by DeathPony
-Derpy and Rarity updates by The Coop (Rarity Tantrum, Derpy sitting fix, Drama Couch fix, rarity sleeping fix, etc)
-Additional Apple Fritter animations by vulcan539
-Archer by vulcan539
-Allie Way (Bowling pony) by Bot-Chan
-Berry Bunch updates (from OC ponies; I reconsidered) by CANDYBAG
-Rarity Fashion show by StarStep
-CMC skipping by The Coop
-Additional Mysterious Mare Do Well and Rainbow Dash animations by DrZoin
-Rarity's Parents by Bot-Chan
-Ruby (Berry?) Pinch by Azure Fang
-Sparkler (Amethyst Star) by vulcan539
-Naked Trixie pixel fix by ???
-Pinkie Pie GDI error (saltoend_right, teleporreappear_right) fixes by StarStep
-Twilight Drag by StarStep
-Iron Will by StarStep
-All of the PNG files have been re-saved with a program that I think will fix any remaining issues with them (randomly being displayed large).


V1.41 TEST 2

-It was sometimes not possible to right click or drag a pony with scaling.  Fixed.
-Clicking on a house with scaling could have led to a crash.  Fixed.
-Effects are now centered properly when scaling is used.
-Ponies now appear in the main menu sorted by their name (as defined in the INI file)
-Main Menu quick find:  After clicking once in the general pony list area, you can press a key to have the menu scroll down to the first pony who's name starts with that letter (just like in the editor).
-There is now a checkbox for "Don't Run Randomly" when creating a new behavior.
-New option for effects and behaviors in the editor:  "Don't repeat animations".  Will force animations to stop on their last frame after one loop, regardless how a gif file is set up.
-Mouseover speech is back.


V1.41 TEST 1

-New feature:  Pony Houses!  (Pony cycling)
These are stored in their own folders like ponies and games, but with a house_ prefix.  Only one is included at the moment:  Trixie's stage

Each "house" can have its own timer (default is 5 minutes) in which one of the following happens by chance:
-Absolutely nothing!
-A pony currently displayed on the screen goes back to the house and enters it. (unless there is only 1 pony left).
-A pony exits the house to join the others on the screen (up to max pony limit as defined in the options menu).

Each house can have its own list of ponies that are allowed to enter it (or leave from it).  For the moment, Trixie has MANY guests crashing at her place... since that's the only art I have for structures.


V1.40 (official - includes test versions listed below)

-Master and MareDoWell have had categories added.  Violet's 2nd "sit" behavior has been renamed so it is not a duplicate.
-The Dr. Whooves from the show (original) has replaced the Dr.Who fan one


V1.40 TEST 2

-Updated image handling and gif reading code from Rooster Dragon.  Result:  Less memory usage, among other things!
-Missing "T" in title fixed
-The options and mini-game menus now has scrollbars if they are resized to be smaller.
-The buttons and other controls on the main menu will always be visible if the window is resized.
-New option: Slowdown factor.  Slows the application (including animations) down by the amount specified.


V1.40 TEST 1

-Angel, Rainbow Dash, and Winona had behavior durations that didn't make sense (Maximum larger than Minimum).  Fixed
-Hoity Toity, 80's Cheerilee and Elsie all had invalid parameters in some of their behaviors for following.  Fixed.

New and updated art:

+Lily by Bot-chan
+Violet by Vulcan539
+Pinkie Pie summersault and Oink, Oink animations by DeathPony
+Daring do by Anonymous (Heh, fitting!)
+Mr Cake by Bot-Chan and Anonycat
+Update to Mysterious Mare Do Well by JavaNut
+Mjolna by Anonymous
+Dr Woof's hourglass twirl by Anonycat
+Rainbow beepbeep by Felix-0
+Master by Anonycat
+Spike's Moustache by Anonymous + Anonycat
+Sleeping Rarity by Miriam the Bat
+Applejack's Nightmare Night costume.
+AJ lasso + pose animations by Starstep and Bot-Chan
+Derpy's "Just don't know what went wrong" by The Coop

-Those of you with very large screens (?) will no longer get overflow errors when clicking ponies.
-The old behavior of hiding the pony when opening up other windows in the editor has been restored.

-Selecting "Return to menu" after the program was "auto-started" will actually show a visible menu.
-Scaling was improperly being calculated in some instances, causing problems with dragging, among other things.

-Taking control of a pony when playing a game did not work properly. Fixed.

-Known issues:
-XP users may see white/gray bars and other artifacts.
-Some images may cause errors like "Caught an generic error in GDI+" in certain setups (XP Machines).
-PNG files are sometimes displayed much larger than they should be (gala dresses...). Some of these images have been disabled for now.

TO DO List:

- Pony "Houses"


V1.39

+New graphics engine by RoosterDragon.  The way ponies are drawn on the screen has been completely re-done, allowing for more ponies at the same time!
+New options in the options menu:  Ponies Always On Top.  Checked:  Ponies walk over everything like they own the place, like they normally do.  Unchecked:  Ponies will walk behind windows that you are working on.
+Added a total selected pony counter to the main screen.
+New option in the options menu:  Suspend and hide when fullscreen applications are running.  (Enabled by default).
+The right click menu is now labeled with the name of the pony the menu was generated for.
+New option in the right click menu:  "Close all copies of X"
+The Add-Pony right-click option is now sorted, instead of being one giant list.
+When dragging, the cursor will be fixed over the specified center of the image (as set in the editor) .  See Derpy or Rainbow Dash for examples.
+New option in the option menu:  Sound Volume control.

-Your settings file will no longer grow continuously every time you save (this also made loading times longer).
-Pinkie Pie now lures parasprites again (somehow the interaction was lost in a previous update.  Again.).
-The user-specified "standing animation" for behaviors that follow should work properly now.  Ponies that reach their destination should wait for a bit using this animation before continuing on (previously they would stop and run in place).
-Checking the option to have ponies teleport immediately if they are outside their allowed area will also affect ponies who are already trying to walk onscreen.
-The program will no longer show up in the taskbar when loading in autostart mode.
-Default pony limit increased to 300 from 75.
-When resizing the main menu, the scroll bars will reset to their initial positions to prevent gaps from forming.
-Snips and Snails have been demoted from "Stallions" to "Colts" in the tagging system.
-A mysterious man named "Jacob" corrected the punctuation for all the ponies and fixed many other issues with the speech text (including correcting the number of "Yes’s in Twilight's line).
-Someone who signed the e-mail "-Panzi" pointed out a lot of duplicate and un-referenced files, which have been cleaned up or fixed.
-The menu icon is now the same as the exe icon.

Art updates:

+Fleur de Lis by Pony Noia.
+Luna's personal guard, by Bot-Chan
+Fancy Pants by Pony Noia
+Mysterious Mare Do Well by JavaNut
+Mrs Cake by Bot-chan.
+Davenport (of Sofas and Quills) by Anonycat
+Uncle Orange, by Anonycat

-Berry Bunch has had some minor fixes to her sitting images applied (white spot removal).
-Additional art for Philomena (From a while ago).
-Derpy Dragging animation by Blackfeathr
-Colgate Sitting animation by Vulcan539
-Pinkaport and PinkieCannon animations and interaction by DeathPony
-Rainbow Dash drag-by-the-tail animations by Bot-chan
-New spitfire (uniformed) animations by Bot-chan
-Twilight image fixes by Bot-chan
-Spike and the door, by Anonycat


v1.38.1
-Fixed a crash on startup for those who don't have DirectX installed.


V1.38

- Ponies now detect the avoidance area (evergreen forest) by means of a 90 pixel hit box around the center of the pony.  This should improve detection of the area and avoid flickering.
-Pony "sliding," for those with Windows XP, has been reduced, but not eliminated (you may still see ponies sliding, but they should wake up sooner, instead of sliding across the entire screen)

-Ponies will no longer get stuck near the middle of the screen if the boundary of the avoidance area happened to be near.
-Ponies would rarely get confused and stop moving when they found themselves in an avoidance area.  Fixed.

-The old teleport behavior is available again through the options menu.  The default will continue to be having ponies slowly walk out of areas they shouldn't be in.
-Ponies should ignore the cursor in screensaver mode, and the cursor itself will now be hidden.
-Screensaver mode now has options for the background:

\Transparent (normal)

\Solid color background

\Custom image background

-Interaction errors no longer show in screensaver mode.
-Effects and speech are disabled when dragging a pony to prevent them from breaking the drag.

-Ponies should get stuck on the cursor less often; they will stop dragging if they find they no longer have input focus.

-Those of you with ffdshow should no longer end up with hundreds of blue icons in your taskbar due to pony sounds.

Pony and art changes:

-Applejack now drops more apples.
-Scratch now moonwalks properly and shouldn't go off stage. Thanks to Darksilver for this.

-Sweetie Belle jumping animation by Cantorlot.
-Scooting Sweetie Bell by StarStep

-New mouseover image for Fluttershy, by Deathpony.

-Improved picture-taking interaction scene image for Photo Finish. 
-Big Celestia now has sounds. Thanks to Darksilver.

-More art for Gummy + Pinkie Pie by DeathPony

-Interactions between Pinkie pie and Surprise, Pinkie Pie and Gummy by DeathPony

-Drama Couch Rarity by Bot-Chan

-Raging Twilight by Bot-Chan

-Philomena by Pony Noia

-Tank by Bot-chan

-Fluttershy Dragging animation by DeathPony

-Fluttershy has had image-centering applied

-Galloping Twilight by InfinityDash

-Mental breakdown Twilight image by ??? (couldn't find this for some reason - let me know).

-Discorded Twilight by StarStep

-Pipsqueak by StarStep


V1.37

Fixes:

-A few ponies had bad images (Silver Spoon, Daisy, Blinky Pie) that were causing crashes with "Generic Error in GDI" for older machines.  Those images have been corrected and no further crashes or "red boxes" should appear.  Hopefully.

-The program will no longer crash with an error about DirectX and sounds even if you have sounds disabled (and even if you don't even have DirectX).

-You can select Mp3 files when making a new pony speech, instead of being forced to pick a WAV file still.

-The code in Pony.Move() has been cleaned up a bit, and several misc logical errors corrected.

Unfortunately, ponies getting stuck and flicking when using Avoidance Areas/Evergreen Forest zones is still an ongoing issue.


V1.36.1

-New/updated art:

-New Gilda by bot-chan
-Rocking Chair for Granny Smith by Bot-chan
-Berry Punch sitting by vulcan539 
-Derpy sitting by vulcan539 
-Updated Pinkie by DeathPony
-Season 2 Luna by Bot-chan (with interaction with nightmare moon)
-Small dancing interaction between Scratch and the Pie family by Pony Noia
  (and dancing(?) Dash too.)
-Suitless Spitfire by Rainbowdutch
-Twilight's balloon by Anonycat
-Updated Raindrop's dress by Starstep
-Pondering Rarity by StarStep

Changes:

New options:
	-You can now choose to disable sounds while in screensaver mode.
	-You can now choose to limit sounds to one at a time, or one per pony.
	-The random pony now has a checkbox to prevent duplicate random ponies (except when you ask for more random ponies than you have unique ones).

-Mathias Panzenböck provided a bunch of sounds from the MLP:FIM Soundboard (https://kyrospawn.deviantart.com/art/MLP-FIM-Soundboard-V7-0-244757196?) and added them to the pony ini files.  
-Note that sounds are now in .MP3 format instead of .WAV.
-In order to support MP3s, DirectX is now used for sound playing.  If you don't have the right DirectX version, you will see that sounds are disabled in the options menu.
-Note that the INI format for speech lines has changed to allow multiple versions of the same file (ogg, etc).  This is for compatibility with Browser Ponies.

-When making changes to speech, the changes would persist even if you didn't save and closed the editor (but not to disk).  Fixed.
-Pinkie can now be properly dragged (dragging behaviors that were set to “do not run randomly” didn't work).
-Spike has had image-centering applied to prevent him from making jittery movements when following Rarity.
-Ponies that run into a barrier or reach their destination will now wait for a short time before continuing (to stop jittering).
-Ponies should not get stuck on the mouse cursor when they walk into it anymore.
-Occasionally the program would freeze on loading.  That should be fixed now.
-Played sounds never actually matched their tooltip speech.  They are now properly played for the right lines. 
-The pony editor now tries to make all filenames and paths in the ini files in lowercase for the sanity of the people who maintain the other ports (that run on case-sensitive operating systems).
-The pony editor will now complain if you made a behavior, speech, interaction, effect, or pony with the same name as another.
-Applejack and Apple Bloom's unusual behaviors (truck, dancing, spinning) have been reduced in probability a bit.
-All saved files (ini, settings, etc) will be saved in UTF8 instead of UTF16 from now on (again for compatibility for those who run different operating systems.)
-You will only get one error message when sounds fail to play, instead of one per try.
-When adding a “random pony” to the roster in the mini-games, you now actually get a random pony instead of just Rarity every time. (https://dilbert.com/search_results?terms=Random+Number+Generator)
-When saving settings in the options menu when ponies were active, your pony counts would be reset.  This will no longer happens.
-Lots of spelling and grammar errors in pony speech have been corrected.


V1.35.2

- The editor will now re-size all of the columns as soon as it starts; previously, it would wait until you made a change first.

-Having multiple sets of the same pony interacting with themselves (you crazy 
people you) would cause an infinite recursion when the interaction(s) ended 
and would crash the program.  This has been fixed, so you can now have ponies 
interacting with their other selves...

- Pony following had a typo in the code that made the Y-coordinate be off when specifying an offset.  Fixed.

- When deleting or changing behaviors in the editor, the preview pony would 
sill use the deleted following behaviors.  Now, the behaviors are re-linked 
after changes are made.

- If there was no image centering specified for the behavior, the default of 
exactly the center of the image should have been used.  Instead, the top 
right corner was. This caused problems with following. Fixed.

- Following ponies would previously stop at a distance of 1/2 their width 
from their target.  This has been reduced to 5 pixels.

-Sapphire Shores found the missing "s" in her name, and no longer walks 
vertically.

- Opal has had image-centering applied so she won't get stuck in a loop when 
following Rarity.

-A couple people pointed out that “Filly Rainbow Dash uses the line "See you boys at the fiish line", obviously missing an 'n' in the word 'finish'.”  -Fixed

-Twist has had image-centering applied so she doesn't jump around for those of you who actually like this pony (I jest).

Art changes:

-Updated Pinkie Pie with some new animations, thanks to DeathPony
-Filly Fluttershy by PsychoSutin
-Lightning Bolt by Starly.
-Sindy (“too much blush” pony) by Pony Noia
-Diamond Tiara and Silver Spoon by Anonycat
-Daisy by Anonycat
-Sir Colton Vines by Anonycat
-Caesar by Anonycat
-Filly Applejack is mighty appreciative of finally getting her own standing 
animation, courtesy of Spuaspeedstrut
-New critter-catching animation for gala-fluttershy by Starstep
-New flying animation for gala-fluttershy by humle


V1.35.1

- The new moving-back-onscreen-instead-of-teleporting did not play nice at all with the avoidance areas, and some ponies would often get stuck.  This should no longer happen as much.

- Image centering did not work properly with scaling.  Fixed.

- DeathPony provided a better-centered Pinkie Pie ini file.  Thanks.


v1.35

+New feature:  Image Centering.  Some ponies (Pinkie Pie...) have animations of greatly varying sizes, which make the pony "jump" around when switching between them.  You can now counteract this effect by specifying a common center for each image in the pony editor.  (The mid-body of the pony works well.)  The ponies with the worst mis-matched images have already had this applied.  It is not needed for ponies with well-balanced image sizes (all the same size).

- Ponies will no longer teleport around if they find themselves to be offscreen (like after switching to a larger image).  They will instead try and walk back onscreen.  The same applies to changing the allowed monitors or applying no-pony zones.
- Previously, adding a new pony when all ponies were asleep with "sleep all" would crash the program when they were woken up.  Fixed.
- When specifying offsets for pony-following, scaling was not taken into account.  Fixed.
- Ponies no longer wake up if you drag them while they are asleep.
- AJ is less fond of her new truck now.
- The right click menu now has quit at the bottom to avoid accidental closure.
- Ponies should now properly display their sleep animation when told to sleep individually.  Previously, they would occasionally be stuck in place walking or flying.
- Pinkie's behaviors have been tweaked a bit.  The copter is a bit less common and fixed some broken/wrong links.
- Only a few ponies have sounds, and those that do had all of them disabled in their ini files for some reason.  Fixed (only for those of you with sounds enabled in the options menu).
- Adding an effect in the editor then immediately trying to run the behavior that triggers it without saving first would crash the program.  Fixed
- The effect centering option "any-not_center" was not recognized, although you could save this setting in the editor.  Fixed.
- The out-of-bounds detection is slightly more intelligent.

New ponies:
- Screwball by DeathPony
- Snips and Snails... by Anonycat
- Sheriff Silverstar by Anonycat
- Angel (+behaviors and interactions with fluttershy) by DeathPony
- Winona by DeathPony
- Surprise (Not a show pony, but since Pinkie Pie did have wings in Faust's original sketches I'm letting this one slide) by DeathPony
- Filly Princess Luna!  So cute! by Asparagus
- Filly Rainbow Dash (VERY well done!) by PsychoShutin (+Starly)
- Granny Smith by Bot-chan

Art changes:
-The original "Dr. Whoof" is included in Dr. Whooves's folder, if you should wish to use him instead.
-Dr. Whooves now has a fez hat, for some unfathomable reason.
-Applejack now has an apple tree to buck
-Zecora sometimes appears with a certain flower.
-New running animation for Dash by SarkinaBox + StarStep
-Princess Pinkie animation by DeathPony


v1.34

- You now have more control of what animations are used when ponies are following - see the new 
controls in the following parameter box in the editor.
	There are two options:
	+Auto select images - automatically select what images should be used based on the pony's 
speed and direction.  This was the behavior prior to this version (unless they were interacting - 
now interactions don't matter).
	+Manual selection - you can select which behavior to use images from when the pony is 
stopped, or moving.
- Thanks to some code from Tursi, ponies are now z-order aware.  Ponies will be displayed "behind" 
or "in front of" each other depending on their position.
- You can now make your own categories/tags/filters, whatever you want to call them, in the 
options menu by pressing the "Custom Filters" button.
- You can now edit tags in the editor.
- Fixed an issue where the editor menu was wrong when using filters.
- Fixed an issue where the menu of ponies would get cleared after saving changes to a pony and 
using filters.
- Pinkie's parasprite herding interaction somehow got deleted from the last several versions.  It 
is now back.
- Effects that were set to follow ponies were in the wrong positions if the scaling was changed (Like for DJ Poni3).  
Fixed.
- Pinkamina's friends will no longer re-spawn everywhere when you drag her (the repeat delay being 
0 caused this).
- When changing art in the editor, the folder shown will be the pony's folder, instead of the base 
folder.
- Pressing the reset button in the options menu will reset the ponies on the menu to normal size 
as well.
- Dinky has appropriate tags now.

Thanks for /dev/tty5 for the following fixes/suggestions:
- Running the screensaver while having 0 ponies set to run in the settings file will now launch 1 
random one instead of getting stuck.  
 - When autostarting or running the screensaver, the main window used to pop up briefly then vanish.  It now doesn’t appear it all. 
- Right clicking and selecting "configure" on the screensaver file didn't work for some versions 
of windows.  Fixed.


- When you run into the dreaded "no pony.ini file!" error, you can now choose to skip seeing that 
message for the rest of the folders.
- The "Mystery_Thumb" file is now included in the source code, instead of just being a mystery for 
those trying to compile the project.
- The screensaver is now the right file type in this release...

-New art:

Even MORE animations for Pinkie Pie, by Death Pony
Fido by Pony Noia
Filly Pinkie by Psychoshutin 
Luna with her Abacus by RQK
Pink Filly Celestia by Asparagus 
Twilight's Mom (Mrs Sparkle) by supersaiyanmikito
Updated Fluttershy sleeping by Asparagus
Colgate sitting by Vulcan539 (+Anonymous)
Filly Applejack by SupaSpeedStrut
Better ponybox for Dr Woof by Anonycat
AJ's truck, by Anonycat
(+ Interaction with Twilight by me... Couldn't resist.)


V1.33

-The app will no longer erroneously try to load your windows system folders as ponies.
-The “cursor avoidance” feature was broken in the last release.  Fixed.
-Ponies with a scale setting other than 1.00 had effects that were not centered properly.  Fixed.
-Using the scale override and setting it to 1.00 had no effect.  This has been fixed.
-Using small scale ponies and using the “return to menu” option gave you a corrupted menu.  This should no longer happen.
-Ponies with invalid category names will no longer prevent you from starting up the app in “autostart” mode.
-Apple Bumpkin no longer walks vertically.
-Releasing the mouse button on a pony you are dragging didn't release the pony from their drag animation.  Fixed (for real this time)


V1.32

+New feature in the options menu: Pony scaling.  You can now select how big or small you would like the ponies to appear on screen.  This can be overridden for individual ponies by including a “scale,1.00” in the ini file - where 1.00 = 1x(normal size), or any other value you choose (5x and 0.25x are probably sane limits).

-Memory usage has been slightly reduced again (especially in the case of having multiple copies of the same pony on screen).
-When going back to the menu your selected counts of ponies were reset, especially when using filters.  This no longer happens.
-The editor did not save your selection for “links to” when adding a new behavior.  Fixed.
-Fixed a crash when returning to the menu after saving a pony in the editor.
-Testing out following behaviors in the editor should work more reliably now.
-Carrot Top no longer trots vertically.
-Right-clicking return to menu while in editor didn't close editor.  Fixed
-The "Show Interaction Errors" checkbox in the options should now do something.
-Dash's mouseover area was off.  Fixed for her and other ponies with large animations.
-Dash would sometimes go into Sonic Rainboom spasms.  This should be fixed.
-A pony being dragged would stay in their drag animation even when the mouse button was released (if you were still hovering over them).  Fixed.
-Slow ponies sometimes didn't bounce off borders and kept on trying to walk into them. Fixed 
-When releasing a pony from being dragged, they will now continue on what they were doing before instead of picking a new behavior.  (This entry was incorrect - it was fixed in 1.33)
-The buttons on the menu no longer float around when the window is resized.
-Some ponies no longer show their gala dresses on mouseover.

Updated art:

Updated Pinkie pie by DeathPony
Apple Bumpkin by Pony Noia
Rover by Pony Noia
Spot by Pony Noia
Big Celestia's Images have been made all a uniform size to prevent "jumping" between animations.
Gala and sleep images for Raindrops (edited from rainbow's)- by AngelKat58 on Youtube.
Sleeping fluttershy (edited from luna's) - by AngelKat58 on Youtube.
Dinky by AnonyCat
Spike replaced with new version by Anonymous
Naked Trixie now has a delay of a few seconds between blinks.


v1.31

-All ponies in games will move at the same speed, instead of Rainbow dominating every match.

-Controlled ponies in games will no longer get stuck on odd animations when idle (this should also 
help with stubborn ponies that don't want to move).

-Added a filter to the main menu.  Ponies are now "tagged" in their pony.ini files.  
	Select "Show all" to show all ponies.
	Select "Any that match" to show all ponies that have at least one of the selected tags.
	Select "Exactly..." to show only ponies that have all of the selected tags.

	For example:
	"Any that match" with "Fillies" and "Unicorns" will show all fillies, and all 
unicorns.
	"Exactly..." fillies and unicorns will only show those fillies who are also unicorns.

-Added a new movement type:  "Dragged".  You can now make behaviors that only show their 
animations while the pony is being dragged with the mouse.

-CPU and memory usage has been reduced (images are not loaded until a pony is activated and the 
menu no longer eats CPU cycles when it is not visible).

-"Trixe" has been replaced by Trixie

-A bug preventing the use of effect and behavior durations of less than 1 second has been fixed.

Updated/new art:

-Twist by Anonycat
-Updated Nurse Redheart by StarStep
-Updated Dr. Whooves by Doctor Blade
-Royal Guard by StarStep (With male pegasi template)
-Gala Fluttershy by humle (with bonus by StarStep)
-Updated Opalescense by StarStep
-Minor updates to Soarn' by StarStep
-Updates to Blues by StarStep
-Fluttershy Stare by StarStep
-Luna jumping by Distoorted
-Big Celestia with Mail by StarStep
-Pinkie Wink by "pony" (deathpwny)
-Inky Pie by Anonycat
-Minor fix to 80's Cherilee by humle


v1.30

-When taking control and not in a game, hitting the control key (right or left depending on which player) will make the pony use it's mouseover mode.
- Much improved art for the ball and goals in games.
- Some ponies have figured out that it's better to try predict where the ball will go, instead of chasing where it is.
- Games actually end when the max score is reached (configurable in the ini file).
- New mini-game: Ping Pong Pony.  Buck the ball onto the other side of the screen to score!  Note that where your pony hits the ball from (hooves/head/body) will make it go off at different angles.  Picking more athletic ponies (faster ones) is recommended.
- Added a check to make sure that the screensaver doesn't accidentally get launched in normal mode, which could result in strange things happening.
- The "Can't pick diagonally only!" bug in the editor came back.  Fixed again...

- NEW PONIES and updated art:

-Apple Fritter by ??? (I left out the 'idle' animation though - she can only trot.)
(The original posting was anonymous)
-Nurse Redheart by Logan (She was also never given an idle animation I believe - She's been sitting in my "not finished" pony folder since May 22nd.  Thanks to Dogostar for reminding me that she and Apple Fritter existed).
- Very excellent updated Carrot Top art by Pony Noia
- Sapphire Shores by Pony Noia
- Gala Dress Pinkie by Humle
- Naked ... I mean, "cape-less" Trixie by Starly
- Fabulous Filly Rarity by RJP
- Soarin' by StarStep and Pie Eatn' by Anonycat
- Gala Pinkie by Humle!
- and dresses for the rest of the mane ponies (static images).  Thanks to StarStep
- Clyde, Sue, and Blinkie Pie by Anonycat!
- Opal by StarStep


V1.29

New ponies:

Carrot Top - By CandyBag
Pokey Pierce - By AnonyCat
(With small interactions between Gummy and Pinkie Pie)

-Right-click add pony-> random gave you a static image instead of an actual pony.  Fixed.
-If window avoidance is disabled, the two check boxes in the options menu that depend on it will be grayed out.
-Take control now works no matter what pony you have focused.
-You can now control two ponies at a time - Different keys are set for "Player 1" and "Player 2"

+ New Feature:  Mini-Game
"Hoofball!  Try to kick the ball into your opponent's goal!  You can either watch the ponies play by 
themselves, or take part by right-clicking a pony and selecting 'take control'!  Player 1 uses the 
arrow keys (movement), right shift (run), and right control (kick toward goal) keys.  Player 2 uses the 
WASD keys (movement), left shift (run), and left control (kick toward goal) keys.  Note that you can 
only kick once every two seconds!  Make it count!"

- Game Known Issues:

	Art sucks:  The ball, "goal", and scoreboard are need to be improved.
	Goalies are pretty stupid.
	Ponies get stuck if the ball is on the outer edge of the screen - drag them around to fix
	Full teams make for one big clump of ponies.


v1.28

-New RANDOM pony option!  A wild-card pony, "Random," has been added that will give you random ponies!  Works like any other in the selection menu.
-New options for window avoidance in the options menu:  Ponies can now avoid each other (more like bounce off each other), or be set to stay in any windows they are placed in.  This may not work well with lots of ponies - some may still 
escape.
-Previewing following behaviors in the editor didn't work.  It now does
-When moving diagonally, ponies can now travel in a range of angles, from 50 to 15 degrees instead of only 45 like before.
-Pony speeds can now be in partial pixels per tick:  Example: 0.5, 1.5, 2.34 instead of just 0,1,2,3.
-Celestia can now fly.
-Gummy now follows Pinkiamina as well as regular Pinkie
-Ponies that have no behaviors will no longer crash the editor.  They will instead be given a default behavior.
-You were never able to pick "Diagonal only" as a movement in the editor.  Fixed now.  In completely unrelated news, Scratch now does not move vertically...
-If for some reason no monitors are set, the first one is added as a default.  This is to help prevent issues with old settings files with new versions.

New content:
-Small new interaction between little strongheart and braeburn, thanks to Pony Noia
-New Pony, Mayor Mare, thanks to: Anonycat

Updated Art:
-Princess Luna sprites have been updated.  She also has a new sleeping behavior - thanks to anonymous
-Photo finish has a better "Da Magics!" animation and also a running one. Thanks to Pony Noia
-Soigne Folio (Photo finish minion) also has a new running animation, thanks to Pony Noia


v1.27

-Saving in the editor will no longer force an immediate reload and quit to the main menu. Instead, you can continue and the reload will occur when you close the editor yourself.
-If you didn't make any changes and close the editor, no reload will occur.
-The editor is smarter about when to ask you if you want to save changes.
-New column in the editor for behaviors:  “Don't run randomly”
	You should check this box for each behavior that shouldn't run by itself (parts of a chain, or a behavior for an interaction).  This will no longer be set automatically by the editor.

-Filly Celestia is able to walk again (what happened is I thought it would be better for her to start walking after the new interactions were done, and linked the end of those to her walking behaviors.  However, the editor said “oh, 	that's a chain!” and marked it as “skip” so it wouldn't play randomly.  The above change should prevent that in the future).

-The “Link Order” column can now show if a behavior is part of more than one chain.
-New option in the right-click menu:  “Return to menu”
-Ponies won't use sleeping behaviors when following (right Sweetie Belle???)
-Fixed a rare(?) crash where GetAppropriateBehavior was called when Current_Behavior was nothing.

-Known issue:  Saving changes then closing the editor causes a memory leak when ponies are reloaded.  Doing this several times will kill performance with lots of ponies loaded.


v1.26

-Added an option to not display warnings regarding interactions.
-Added an option to completely disable speech, including hover-over text.
-The avoidance zone ("everfree forest") wasn't working if ponies were not on your primary monitor.  Fixed.
-Ponies will no longer trigger interactions with themselves... (different copies still work, however)
-Errors will now be displayed when trying to play a sound that is in the wrong format, or with other problems.
-You can now drag effects around if you have dragging enabled in the options.
-Random speech works again!
-Pegasi will now fly up to reach follow targets instead of just walking straight up, which looked funny.

-The code has been cleaned up a bit, removing dead code and commenting areas that are not obvious in their purpose.

-Pinkie's parasprite herding interaction had a typo that caused it to ignore the reactivation delay.  Fixed
-Parasprites also follow a bit more slowly for better effect.

Art additions and changes:

-The original DJ Poni3 is no longer included.
-New ponies:
	Big Mac, thanks StarStep
	Little StrongHeart, Pony Noia
	Rose(luck), Anonymous
	Shadowbolt - Pony Noia/Glamador
	Caramel, Starly
	Waiter pony - Anonycat
	Raindrops - Starly

-New art:
	Dancing applebloom, Pony Noia
	Sitting Blues, StarStep
	Fixed filly celestia sleep - Asparagus
	updated Bonbon and Lyra - Starly
	New Hoity-Toity art - Pony Noia
	Updated Zecora (missed a bit in the last version) - RJP
	Updated Derpy - RJP


-New interactions, thanks to Pseudo Hoof
	Big Celesita/Nightmare Moon
	Rainbow Dash/Gilda


v1.25

- Black/white boxes around effects should be eliminated now.
- You can now select one pony to sleep at a time.
- You can now add ponies one at a time from the right-click menu.
- You can now have a behavior set to speed 0 and movement "none" while having a follow target.  The pony will stay still but always turn to face the target.
- Timing system has been redone - effects and behaviors should last the expected amount of time even when the system is under load and missing a few ticks of the timer.
- Parasprites will no longer get stuck in a loop (behaviors they have a target that doesn't exist don't start)
- interactions didn't match behavior names if they were uppercase - fixed
- Ponies no longer 'reset' after interactions finish - they carry out the last behavior to the end before selecting a new one.
- Multiple ponies engaging in separate interactions at the same time would not follow the right pony.  Fixed.
- Random speech was interrupting scripted ones.  This should be reduced now.
- Ponies already in an interaction now won't start a new one with other ponies.
- Ponies were exiting interactions if they ran out of behaviors but were only targets.  Now they will stay flagged as in an interaction until the initiator exits theirs.
- Interactions where checking against lists of n^2 ponies instead of n.  Fixed (slight performance boost).
- Trixie would only play her fireworks once on mouseover.  Fixed (for other ponies too).
- Changed the way ponies decide on a direction to go (shouldn't have a noticeable effect other than saner code)
- You can now set a "reactivation delay" for interactions in the editor, to prevent interactions from happening too often.
- Link order in the editor is now shown as Chain_number-Link_Number (1-1,1-2,2-1,2-2,etc) to help with ponies having multiple separate chains.
- When adding a behavior in the editor, making a behavior link to another will not automatically set it to "skip"


Changed behavior types:
-twilight book reading -> sleep mode
-sweetie bell cloud sing -> sleep mode
-scoots cute basket -> sleep mode

New interactions - Pseudo Hoof
	-Luna and Filly Celestia chatting
	-Theme song singing with the mane six and big Celestia!
	-Cutie Mark Crusaders

-New pony: Blues - Anonycat
-New pony: Hoity-Toity - Pony Noia
-New Pony: Braeburn - Anonycat

-New apple bloom animations - Pony Noia
-Bigger Pinkamina Friends - Anonycat
-Fixed filly twilight -RJP
-bouncing gummy - by Anonycat
-Elsie speed running - Pony Noia
-Stella speed running - Pony Noia
-Slightly updated Lyra walking - Starly
-RBD's cloud-sleep fixed (again)


v1.24

+Added a screen saver!  See web site "Documentation" page for installation instructions.
+New "Sleep" mode - pauses all pony movement.
-Some ponies previously couldn't pass between screens.  Fixed
-a "Minor grammar gripe" with Sweetie Belle has been fixed.
-The appearance of black/white boxes around ponies and effects in Windows 7 should be reduced somewhat.
-Added more buttons on the main form to make it more obvious that you can save your setup of how many ponies of each type you want.

Updated ponies:
Big Celestia - Now with flying and idle animations.  Thanks oppl jok
Little Celestia - Now with improved standing/walking animations.  Thanks Asparagus (although I disabled the Trollestia behavior...)
Improved Rainbow sleep animation - thanks to ?
Pinkamina "friends" - Thanks, Anonypony!
Spitfire - Thanks to Rainbowdutch
Filly Twilight - Thanks RJP
Photo Finish Sounds - Thanks Rainbowdutch 
Colgate/Romana/That toothpaste Pony - Thanks RJP

(Many other ponies have been updated to work with the sleep feature - they just needed their idle animations to be set to "mouse over" - otherwise they would be stuck walking when sleep mode was used)


v1.23

-error checking wasn't enabled when loading settings.  fixed.

-The SplitWithQualifiers() function wasn't handling fields with leading spaces correctly.  The result was that you couldn't save your preferred number of 80's Cheerilees and Sweetie Belles - they both had a leading space in the name (now removed).  Fixed 

-Changed memory leak fix from previous version to prevent possible errors with images.  We were manually disposing of images when no longer needed, but other threads (like tooltip speech) may still have needed them causing a small window of opportunity for errors.

-New feature - autostart command line.  "DesktopPonies.exe autostart" will automatically launch ponies with the settings in the INI file (or defaults if no settings are there).


v1.22

-Stella's walking animation had some non-transparent parts.  Fixed
-Elise would sometimes jump around while changing animations.  Fixed
-Aloe and Zecora had broken following behaviors due to a change in the last version.  Fixed
-Photo Finish's camera (that Elise carries) should work better now.  The three animations have been combined into one.
-Cheerilee has been given back her missing "e"
-A massive memory leak in pony.paint() was pointed out by a brony.  Thanks!  This has been fixed.
-A debug line was inadvertently left in a few versions back.  Removed.
-Distance was not being calculated correctly for interactions, causing them to activate at the wrong times.  Fixed
-The start speech for the first behavior for interactions was never being played.  Photo Finish now says her "DE MAGICS" line.
-The mouse doesn't interrupt interactions anymore (unless you click on ponies).

New/updated ponies:

-80's Cheerilee
-Updated Derpy
-Sweetie Belle
-Scootaloo
-Additional art for Dash
-Candy Mane
-Elsie as noted above
-Stella as noted above


v1.21

Fixes:
-You couldn't create a new interaction in the editor.  Fixed.
-You couldn't delete the last interaction for a pony.  Fixed.
-Making a pony use a specific image when following was a challenge.  Now, if the follow behavior has movement set to "all," they will always use the images assigned to it, except when stopped.
-Trying to set a behavior's speech to "none" in the editor would actually try and link it to a speech named "none", instead of making it blank.  Fixed
-Modified how following works in interactions.  Should work properly now with interactions behavior chains having multiple follow targets (goodness Elsie...)
-Effects that were very short duration and had a repeat delay of 0 (meaning play only once) would play over and over again.  Fixed

New art:
-Apple Bloom
-Cheerilee
-Octavia
-Photo Finish (and her 3 helpers, Elsie, Soigne, and Stella)
	(New interaction also with Fluttershy)
-Berry Punch (with optional drunk version included...)

Updated art:
-Applejack
-Doctor Whoof
-Twilight Sparkle

Added sounds for Pinkie and Fluttershy, if you have them enabled


V1.20

Many bug fixes to the pony editor.

Updated art:  Gummy, Derpy!


v1.19

+New feature:  Pony Editor Gui!

-Behaviors that are part of a chain and shouldn't be used individually are no longer selected to be used when using "Take Control".  This fixes Twilight's broken behavior in that mode.
-When attempting to follow an object, but unable to due to a barrier of some kind, ponies will now sit and stare longingly at their object of desire until they can move again.  They used to just forget about it and do something else
-Effects are now enabled on mouseover
-Ponies will now try to figure out what a good place to move to would be first before teleporting out of an avoidance zone (help reduce ponies going crazy all over if you set the avoidance zone too big).

Updated ponies:  Twilight (ini file fix for take_control), Luna (art), Trixie (fireworks!), Rainbow Dash (now includes original PSD files, no other changes)


v1.18
Updated ponies:  Gilda, Nightmare Moon, Parasprite, Princess Luna, Trixie
Fixed ponies:  Pinkamina no longer walks vertically.  Derpy walks a bit faster

New features:  
-Ponies will now "bounce" off of walls/boundaries instead of selecting a new behavior.  This should help not get clumped up in corners, and just look better.
-There is now an option to avoid other program windows.  Note that this is disabled by default; find it in the options menu.
	*this may take quite a bit more CPU usage than other options*
	*note that it doesn't work well with too many ponies on the screen - they end up overlapping and obscuring each other’s view of what windows are there*


v1.17

- Sometimes, if the behavior duration didn’t exactly match the gif duration you could get both out of sync.  Fixed (pony.paint() and pony_form class change)
- Pony speaking on mouseover was broken.  Fixed
- The right click menu on ponies would often disable pony speaking.  Fixed (ponyform + options changes)
- New twilight animations.  
- You can now say “any” or “any_notcenter” for effect directions and centering.  Parasprites have been improved also as a result.
- The tooltip crash believed fixed in the last version is now fixed for good...
- Previously, coordinates specified when also specifying a pony or effect to follow were ignored.  Now, these coordinates specify the offset from the center of the target object to go to.
(meaning to can have the follow point be to the right, left, up, down, etc from the actual object.)
- “Horizontal_Vertical” never actually worked as a movement for behaviors.  Fixed.

+New Feature:  Interactions:

'InteractionName, PonyName, probability, proximity_activation_distance, {Targets}, random_or_all , {behaviors}
'
'Explaination:
'Interaction - the name of the interaction
'Ponyname - The name of the pony initiating the action
'Probability - The chance of the interaction occurring.
'        1.0 = every time the pony is in range.
'               0.001  = low chance.
'proximity - the distance the pony would have to be, or closer, to activate the 'interaction.
'        you can say "default" to leave as normal.
'{Targets} - the list, in brackets, of ponies that can be interacted with
'random_or_all - either "random" or "all"
'     random means pick one pony to interact with from the list
'    all means the interaction occurs with the entire list at once. ‘interaction.  "Any" keyword allowed.
'{behaviors} - the list, in brackets, of behaviors to choose from (one is picked randomly) which is activated for both the initiating pony and all selected target ponies.

Example:
Pinkie_Lures_Parasprites, "Pinkie Pie", 0.05, default, {"ParaSprite", "Princess Celestia"}, all, {parasprite_follow_circle}


v1.16 (skipped 1.15 to avoid confusion with a non-official version)

- If the tallest pony in a row on the menu screen was the first one in that row, then the placement of the images would be incorrect, and overlap the next row.  Fixed (small change to Add_to_Menu() and Main_Resize())
- Hopefully fixed a crash with the following error:
"System.ObjectDisposedException: Cannot access a disposed object.
Object name: 'PictureBox'.
at System.Windows.Forms.Control.CreateHandle()
at System.Windows.Forms.Control.get_Handle() "
... caused by a tooltip (pony speak) being active while a pony form was closed.  Modified end of Pony_Form class.
- new art - fluttershy, new Scratch, Lyra, big Celestia, multiplying parasprites, and Twilight (partially)
- Zecora's name is now Zecora.
- When moving on to a linked behavior, effects that had duration 0 (only last until behavior ends) wouldn't clear.  Fixed (change to Pony.Paint() and Effect_Form class change)
- Using the right click menu on a pony to select an option would sometimes disable effects even after the menu closed.  Fixed (change to Pony_Form.ContextMenu_ItemSelected())
- Effect duration and repeat delay values, as well as behavior durations can now be specified to the millisecond. (0.333 seconds, for example).
   (They are rounded to the nearest 1/33 of a second).
  However please be careful with this, as very low values could kill performance.
  Values of less than 0.030 are pointless as this is the max speed the program updates at.
+Sounds! (I find this to be very annoying so they are off by default).
    Note that only .WAV files are supported.

Note that the Pony.ini format has changed slightly to accommodate this feature.

'Lines for the pony to say (when hovered over, small random chance, of specific lines for         specified behaviors)

'speakingline1,name, "thing to say", "wav file to play.wav", true if this is for a behavior         and shouldn’t be ‘played randomly

'(if this is just a normal random thing to say, you can omit the name, filename, and         true/false)

Example:  Applejack plays a sound file when she stops galloping, noticing that she         dropped apples along the way.

Normal Sound:

Speak,"Hey there sugarcube!"

Sound for a behavior:

Speak,giddyup_sound, "Yeee...", "", true

Speak,gallop_sound, "Haw...", "didn't see that one comin.wav", true

For the behaviors that speak, change the line from containing the actual text, to the         name of the “speak” line that has it:
   

Behavior,giddyup, 0.05, 1, 1, 1, rear_right.gif, rear_left.gif, None, gallop,                 giddyup_sound, , false,0,0,""

Behavior,gallop, 0.05, 3, 1, 6, gallop_right.gif, gallop_left.gif, Diagonal_Only, , gallop_sound, , true,0,0,""


v1.14

- Effects weren't showing if there was a window between them and the desktop.  fixed
- Effects are now disabled if the right-click menu is active to prevent it from closing due to them
- Users were getting an error: "Can't save to disk.  Details: Unable to translate Unicode character \uDF6C at index 65 to specified code page."
This should be fixed now as the save file won't use ANSI code pages by default, but instead uses UTF32 (unicode).

- if you tried to save settings after already having ponies displayed, and did not have at least one of each pony on the screen, saving would fail. fixed.
- Effects now only close on behavior change if their duration is 0. (Dash’s rainboom will stick around for a while even if she stops because she can’t go any further)
- Effect line format in poni.ini files has changed.  You now specify settings for going left and right.  (See Rainbow’s updated file for examples.)

+ Updated Rainbow Dash!
+ Updated Pinkie Pie!
+ Updated Rarity!


v1.13

-”Take Control” should now do a better job of selecting animations to play.
- The pony right click menu now changes the menu from "Take control" to "release control" if you already have control...
- Ponies will try to get your attention if you attempt to interact with a pony when in control of another.
- The ponies have now learned how to go to specific points on the screen and to follow one another around (for fun: try placing some parasprites with some applejacks!)
- New ponies:  Aloe, Lotus, and Zecora!

****PONY.INI BEHAVIOR CONFIG CHANGED SLIGHTLY****
****Custom ponies will have to be modified to work****

-    All behaviors with allowed movements of NONE or MouseOver should have their speed set to 0 for take control to work
-    ***All behaviors must list both right and left images in that order. (removes the need to have two similar behaviors for left/right)***
-    Old linked behaviors now need 3 new parameters to work:   ,0,0, “”
    (comma, zero, comma, zero, open quote, closed quote).


v1.12

- smaller mouse_over images causing infinite loops when the resumed image is bigger. (more pony spasms when mouse cursor is near) - Fixed
-Trixie's mouseover has been set to fireworks, because that's what she'd do when getting attention
-Improved graphics for main cast members that didn't already have them (most notably pinkie).
-Ponies will not talk with the right click menu was open (they were stealing focus and closing it).
+Newer Bon Bon
+parasprites added
+spike added
+ New feature:  Effects (see AJ’s ini file for details):

    Effect Examples:

'The following makes AJ drop 1 apple per second when she gallops that stays
'behind for 3 seconds. (she's a silly pony, huh?)  This is the only effect currently in use, because
‘a bad apple graphic is all i could make.
Effect, "Apple Drop", gallop_r, apple.png, 3, 1, bottom_left, center, false
Effect, "Apple Drop", gallop_l, apple.png, 3, 1, bottom_right, center, false

'When Dashing, Rainbow Dash leaves behind a sonic rainboom which stays in place.
'The delay_Before_next is 0 meaning don't repeat to leave only one.
'The rainboom animation sticks around for 5 seconds (the gif animation should be the same duration).
'The image is created to the right of rainbow as she moves left,
'and in centered on the left of the image, so it does not overlap with her.
‘(rainboom.gif not included)
Effect, "Sonic Rainboom_left", RainBow_Dash_Left, Rainboom.gif, 5, 0, right,left, false

'Rainbow dash leaves a rainbow trail behind her, that consists of two parts:
'the first is a trail that is animated to fade over time, and stays behind her
'  (getting the timing right will be tricky)
'the second is a trail that follows rainbow to allows the appearance of a single
'continuous effect
‘(working rainbow trail images not included)
Effect, "Rainbow Trail_left", dash_l, left_rainbow.gif, 2, 1, right, left, false
Effect, "Rainbow Trail_left_follow", dash_l, left_rainbow_follow.png, 0, 0, right, left, true


v1.10 -> v1.11

- Ponies sometimes flicker when changing behaviors. - Fixed.
- HUGE performance improvement for those with Windows Aero (win7 and Vista) on and lots of ponies on screen.
- Lyra no longer moves vertically, because the jumping was freaking Bon Bon out.
-In return, Bon Bon agreed to face the right way in the menu.

- Ponies initialize randomly over all allowed screens one at a time, instead of
all at once all in one place.

- Shortened durations for all ponies - it looks like everyone was mostly copying the initial Derpy file.  Do you really think Pinkie Pie can stand still for 15 seconds?  I don't.   This should also help with ponies ending up on screen edges.


v1.09 -> v1.10

v1.09 had a bug where ponies could not escape from a screen they should not have been on, or an ‘everfree forest zone’.  They would just spaz out.  Fixed now.


v1.08 -> v1.09

-Misc typos in ALL of the pony.ini’s fixed
-AJ had a backwards animation, fixed
-AJ was galloping way too often (in my opinion), so toned her down a bit.
-Ponies falling asleep at the corners of the screen - fixed  (separated and cleaned up code for cursor halting - it was a mess...)


v1.07 -> v1.08

- Added new graphics for a bunch of ponies.  Tell me if I am missing any updates!
- Menu UI fixes provided by 'Velocity'

- When you "take control" of a pony, the mouse will still block movement - fixed! (cursor avoidance is disabled for controlled ponies.  
(Also note that taking control blocks the right click menu of other ponies - the one in control will keep trying to take focus back)

- Settings will now stay even on closing/reopening the options menu.

+ New feature:  You can save your settings (including # of each type of pony) in the options menu!  This is saved in a file named "Settings.ini" in the same folder as Desktop Ponies.exe

This file is automatically loaded (if present) on startup.

Known issues:  Ponies sometimes get stuck in the corners of the screen under certain conditions.  Reason:  messy code.  Cleaning up now.


v1.06 -> v1.07

- mouse_over doesn't play gifs??!?!? - FIXED
- now with 50% less pony talking by default
- talking interrupts take_control - FIXED

New options menu (you can even leave it open and change settings on the fly)!

Has the following options you can change:
- optional cursor avoidance - also improved.
- cursor avoidance zone size
- pony talk chance
- max ponies
- Pony dragging disable option

Known bugs:  Settings options, then closing, and reopening the options menu through right-click resets settings to default.


v1.05 -> v1.06

The horribleness of the last version should be reduced.
I think the crashing was caused by SelectBehavior() being recursive, and people having bad luck (or too many ponies).


v1.04 -> v1.05

A brony, ‘Velocity’ generously donated beautifying code for the UI!  ...and then I muddied it with the controls from v1.04...


v1.03 -> v1.04

New features:

-That folder separator thingy that broke things for people crazy enough to run under linux+mono is fixed (i think, can't try it for myself...)
+Cursor avoidance. Ponies will avoid running under the cursor, and stop still if you hover over them.
+Monitor selection - if you have more than one monitor, you can select which ones ponies will go to, or all!
+"The Everfree forest" - you can designate a no-pony-zone on your screen that ponies will never enter!

...and, all of the ponies are added that i've seen until now!


v1.02 -> v1.03

-Fixed linked behaviors - only the first one would link (exit sub <> exit for, oops)


v1.01 -> v1.02

+Added "linked behaviors."  See Derpy's INI file for an example.
-Fixed the graphics for all ponies but twilight so they won't "float-walk" anymore