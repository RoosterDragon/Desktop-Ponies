# Technical Documentation

The following details some of the file formats employed by Desktop Ponies, for those interested in using these files in other ports or implementations of the ponies or for those masochists who would prefer to edit a poorly designed text file format rather than using the editor for creating their ponies :).

## Ponies

The "Ponies" directory contains individual directories for each pony. The "Random Pony" directory contains a pony that just provides images for a selecting a pony at random and should not be considered as part of the set of ponies that can be selected.

The name of each directory is the unique and case sensitive identifier for a pony. For portability reasons ponies with names differing only by case should be avoided where possible.

## pony.ini File Format

The file named "pony.ini" (always lowercase) inside each directory specifies the configuration for the pony. It should be noted that despite the extension the format is not actually INI. The extension is kept for compatibility.

This is a UTF-8 encoded text file. Line endings are given by any of a line feed, carriage return, or carriage return immediately followed by a line feed.

The file is interpreted line-by-line. Empty lines or those containing only white-space characters are ignored. Lines that begin with a single quote character are comments. These are ignored. All other lines are interpreted, the order of lines is not important.

In the document, any elements referring to coordinates or vectors follow the usual conventions for coordinate systems in computing. (0,0) represents the top-left point. X increases towards as you move right and Y increases as you move down (note that this is the opposite of the Cartesian system where Y increases as you move up).

### General Line Format

Lines not otherwise ignored are interpreted as follows:

The first comma character is found. The text before this comma is a case insensitive identifier for the type of line. If no comma is found the line is ignored.

The identifier specifies what configuration information the line contains. If an identifier is not recognized the line is ignored.

Future identifiers may be added as a way of extending configuration files, and thus an unrecognized identifier is not an error. Equally, more elements might be added to an existing identifier. If an implementation does not recognize them it should ignore the extra elements but still use the rest of the line.

Each line can be roughly parsed according to the CSV format. However they do not strictly adhere to this format and some munging of the resulting elements may be needed to get something usable if you are attempting to interpret the lines in this manner.

Whitespace inserted around separating commas or quote marks is significant! Avoid inserting extra whitespace.

### Name

Should appear: 0 or 1 times.

#### Format

    Name,*DisplayName*

#### Examples

    Name,Twilight Sparkle
    Name,"Twilight Sparkle"

#### Elements
* *DisplayName* - The display name of pony. This may be quoted.

If this line is not present in the file, the display name of the pony will be the same as its unique identifier (the directory name). If multiple lines are present then the result is undefined.

Recall the directory for the pony serves as its unique identifier. For example, you might have "Twilight Sparkle" and "Twilight Sparkle (Filly)". This allows you to give a descriptive name to all ponies so the user can tell them apart.

The display name provides the plain name of the pony without the descriptive elements for use in speeches.

For example, if you don't provide this line in the config for "Twilight Sparkle (Filly)", then any speech will come out as:
    Twilight Sparkle (Filly): Hello!

However you can override this to provide just the plain name by adding this line:
    Name, Twilight Sparkle

Then any speech from filly Twilight comes out as
    Twilight Sparkle: Hello!

### Categories

Should appear: Any number of times.

#### Format

    Categories,*Tag1*,*Tag2*,*Tag3*,*...*

#### Examples

    Categories,"Main Ponies","Mares","Earth Ponies"
    Categories,Supporting Ponies,"Stallions",Unicorns
    Categories,Pets,Cat

#### Elements:
* *TagN* - A case insensitive tag that categories the pony. Each tag may be quoted.

A pony can have any number of tags applied to it to categorize it. You can use any tags you want. For example if you have several OC Griffons you might add the tag "Griffon" to each one to group them together.

There are several commonly used tags that are recommended to allow the user to quickly filter ponies. These are:
* Main Ponies
* Supporting Ponies
* Alternate Art
* Fillies
* Colts
* Pets
* Stallions
* Mares
* Alicorns
* Unicorns
* Pegasi
* Earth Ponies
* Non-Ponies

If you apply these tags as appropriate, Desktop Ponies will be able to show you pony in search results.

This line is optional, and it can be left out (or no tags can be provided in the list) if desired. If it appears multiple times, then all tags from all lines are used. You can obviously combine these into one line if you like.

### Behavior Group

Should appear: Any number of times.

#### Format
    BehaviorGroup,*GroupNumber*,*GroupName*

You must specify both elements.

#### Examples
    BehaviorGroup,1,Gala Dress
    BehaviorGroup,100,"Suited"

#### Elements
* *GroupNumber* - A behavior group number. (Integer between 0 and 100 inclusive.)
* *GroupName* - A name for this behavior group. This may be quoted.

Behavior groups are explained in more detail under the behavior section. Behaviors may be put into numeric groups. However it is often useful to provide a more descriptive name for each group which can be done by using this line to define a name for each group individually.

For example if you have a pony that puts on their dress for the gala, and a gown for a ball, you might create two groups. You can add these two lines to give them names:
    BehaviorGroup,1,Gala Dress
    BehaviorGroup,2,Ball Gown

This will make dealing with the groups easier, since you won't have to remember what 1 and 2 refer to.

These are optional. If a group lacks a name the number will be used instead. You should ensure you give each group at most one name, or the results are undefined. If an invalid group number is given the line will be ignored.

### Behavior

Should appear: 1 or more times.

#### Format
    Behavior,*Name*,*Chance*,*Max Duration*,*Min Duration*,*Speed*,*Right Image*,*Left Image*,*Movement*,*Linked Behavior*,*Start Speech*,*End Speech*,*Skip*,*Target X*,*Target Y*,*Follow Target*,*Auto Select Follow Images*,*Follow Stopped Behavior*,*Follow Moving Behavior*,"*Right Image Center*,*Left Image Center*",*Prevent Animation Loop*,*Group*,*Follow Offset Type*

#### Defaults
    Behavior,*Name Required*,0,15,5,3,*Right Image Required*,*Left Image Required*,All,"","","",False,0,0,"",True,"","","0,0","0,0",False,0,Fixed

You MUST specify at least all the elements up to and including movement. You may rely on the defaults for subsequent elements and can leave them off if desired. The defaults for elements up to and including movement are the values Desktop Ponies will substitute if that element is invalid. These particular defaults are specific to Desktop Ponies only and are subject to change.

#### Examples
    Behavior,"stand",0.15,15,5,0,"stand_right.gif","stand_left.gif",MouseOver,"","","",False,0,0,"",True,"","","50,34","47,34",False,0,Fixed
    Behavior,"put-on-dress",0,3,3,2,"dress_on_right.gif","dress_on_left.gif",All,"wear-dress","","",True,0,0,,False,"","","50,36","47,36",False,0,Fixed
    Behavior,"ride-start",0,3,3,2,"trotcycle_right.gif","trotcycle_left.gif",All,"ride","","",True,10,20,"AnotherPony",False,"stand","walk","50,36","47,36",True,1,Mirror

#### Elements
* *Name* - A case insensitive name that uniquely identifies the behavior for this pony. This may be quoted. If this name is not unique then it will still work but references to it from other areas may not work. For example, if an effect is attached to the "stand" behavior, but there are multiple names with the behavior, then the effect may not fire since it can't be sure which behavior it was supposed to be applied to.
* *Chance* - A decimal number between 0.0 and 1.0 inclusive. This is the chance relative to all other behaviors that this one is selected for use when a pony changes behavior.
* *Max Duration* - The maximum number of seconds the behavior should last. A decimal number between 0.0 and 300.0 inclusive. When a new behavior starts, a uniformly distributed random value between the minimum and maximum duration will be selected as the duration for the behavior.
* *Min Duration* - The minimum number of seconds the behavior should last. A decimal number between 0.0 and 300.0 inclusive. Should be less than the maximum.
* *Speed* - The speed at which the pony should move. A decimal number between 0.0 and 30.0 inclusive. To get the speed of the pony in pixels per second, multiply this value by 100/3.
* *Right Image* - The name of the image file to be used when the pony if facing to the right, which should be a file in the pony's directory. This may be quoted. This should match the casing of the file so that the config can be ported between Windows and Unix systems without issues. If the file cannot be found the behavior will be ignored.
* *Left Image* - As above, but for facing left.
* *Movement* - Controls the directions in which a pony can move. The pony will still need a speed greater than zero to actually move. This can be one of a series of values. These values are case insensitive but it is recommended to use the exact casing in case other implementations rely on it.
    * None - The pony cannot move (even if the speed is greater than zero).
    * Horizontal_Only - The pony can only move on the horizontal axis.
    * Vertical_Only - The pony can only move on the vertical axis.
    * Diagonal_Only - The pony can only move on the two diagonal axes. When movement starts a random angle of 45 ± 30 degrees is chosen which determines the angle of the axis along which the pony travels. The pony may travel in either direction along the axis.
    * Horizontal_Vertical - Allows horizontal or vertical movement. A uniform random selection is made between these two options.
    * Diagonal_horizontal - Allows diagonal or horizontal movement (note the lowercase 'h' in horizontal). A uniform random selection is made between these two options. The random angle for the diagonal axis is chosen to be between 15 and 45 degrees from the horizontal axis. The pony may travel in either direction along the axis.
    * Diagonal_Vertical - Allows diagonal or vertical movement. A uniform random selection is made between these two options. The random angle for the diagonal axis is chosen to be between 15 and 45 degrees from the vertical axis. The pony may travel in either direction along the axis.
    * All - Allows diagonal, vertical or horizontal movement. A uniform random selection is made between these three options. If diagonal movement is selected, the rules for choosing a diagonal axis are the same as those for diagonal only movement.
    * MouseOver - Implies no movement. When the mouse is hovering over a pony a stationary behavior is automatically used whilst the hover is in effect. You can specify a behavior to use instead by using this movement type. This can be helpful if you have a specific mouseover animation to use. The behavior to use is selected from each set of behaviors in a group, so you can specify a behavior per group (if you specify more than one, then the result is undefined). A behavior with this movement type can still be used like a normal behavior, so if you to make sure it only when the mouse is over the pony, be sure to set the skip flag. Ponies in interactions will ignore mouse-over, but can still be dragged.
    * Dragged - Implies no movement. Like the mouse over option, this specifies a specific behavior to use when the pony is being dragged by the mouse. If one is not specified then if there is specific sleep behavior that is used. If neither of those is used then the mouse over behavior for the group is used.
    * Sleep - Implies no movement. Like the mouse over and dragged options, the sleep option can be used to specify a behavior to be used specifically when a pony is put to sleep. Unlike those options, only one sleep behavior is required and they are not specified per group. Ponies that are sleeping will ignore mouse-over, and can still be dragged but will continue to sleep whilst doing so.
* *Linked Behavior* - This can be used to chain behaviors together to create sequences. The name of another behavior can be used here and when this behavior finishes it will start the linked behavior rather than choosing another one at random. This may be quoted. For example let's say you have a roll animation but it has some short animations for the pony to transition into and out of the roll. You can set up the roll-start behavior to run at random, having that link to the roll behavior. That can then link to the roll-end behavior. The roll end behavior does not have a link, allowing a new behavior to be selected at random again.
* *Start Speech* - This refers to the name of a speech that should be run when the behavior starts. This may be quoted.
* *End Speech* - This refers to the name of a speech that should be run when the behavior ends. This may be quoted. Be careful using this setting as if the next behavior has a start speech, then that will run and this end speech will not be seen.
* *Skip* - A Boolean flag ("True" or "False" - case sensitive). If False, that behavior can be selected to be used at random (the chance value can be used to adjust the likelihood the behavior is selected). If True, this prevents the behavior being selected for use at random. This is useful if you need to prevent certain behaviors being used at random (e.g. a specific mouseover animation, or something intended to only be used as part of a chain of linked behaviors).
* *Target X* - An integer that specifies the horizontal coordinate of a target. If a pony target to follow has been given these coordinates represent an offset (see the Follow Target element for details). Otherwise if the coordinates specify something other than (0, 0), then this represents a target to reach relative to the area ponies are allowed in. The co-ordinates are given on a 0-100 scale along each axis. e.g. (100, 100) specifies the bottom-right corner of the area should be reached, whilst (50, 50) specifies the center of the area. You can use coordinates outside this range to target areas off-screen, e.g. (-5, 0) will take the pony outside the left edge. If the coordinates given are (0, 0) and there is no follow target, then this specifies that no target is to be reached and the pony is to move normally (which does unfortunately mean you cannot exactly target the top-left corner of the area).
* *Target Y* - An integer that specifies the vertical coordinate of a target.
* *Follow Target* - This may be set to a name of a pony to follow. This may be quoted. If set and that pony exists then this pony will attempt to follow the specified pony around for the duration of the interaction. The target coordinates are used to specify an offset from the target. For example if the coordinates are (0, 50) then a point 50 pixels below the center of the target pony will be sought out. You can use this to provide some spacing between ponies. See the Follow Offset Type element for options regarding how the offset is treated when the ponies switches their direction of facing.
* *Auto Select Follow Images* - A Boolean flag ("True" or "False" - case sensitive). If True, some behaviors are selected automatically to be used when the pony is following a target. Two behaviors are selected, one to provide the images to use when the pony is moving towards the target, and one to provide images when stationary because they have reached the target. The behaviors are selected primarily depending on their speed and group. The images for these behaviors are then used as the pony moves around, but everything else about the behavior is ignored. If you would like to control the behaviors to use to provide these images yourself, set this element to False and provide names for behaviors in the Follow Stopped Behavior and Follow Moving Behavior elements.
* *Follow Stopped Behavior* - If a target is being sought, and automatic image selection is disabled, the images from this behavior will be used when the pony has stopped because they have reached their destination. If an invalid value is given then automatic selection will be used. This may be quoted.
* *Follow Moving Behavior* - If a target is being sought, and automatic image selection is disabled, the images from this behavior will be used when the pony is moving because they have not reached their destination. If an invalid value is given then automatic selection will be used. This may be quoted.
* *Right Image Center* - A quoted vector consisting of two integer points, e.g. "10,20" or "-5,0". By default, the natural center of an image is used as the location of the pony. When a pony changes between facing left and right or switches into a new behavior, the center of the new image is aligned with the center of the old image. This is intended to ensure the pony tends to appear to occupy the same location as it switches between images. However for some images the default of using the natural center of the image does not work. For example if an image has a pony pulling a wagon, the natural center will lie between the pony and the wagon and not on the pony. When the pony switches images it will appear to jump in location as a result. You can override the use of the natural center for these images by providing a custom point of the image that represents the center to use. If you set this point to be where the saddle would be placed on the pony in all the images they use, you can avoid these jumps because the images will now align on this point. If this value is "0,0" then the natural image center is used, for all other values this is assumed to represent a point on the image measured in pixels. For example if the image is 200x100 pixels, the natural center is (100,50). You could use "200,100" to make the center the bottom-right point of the image. You can also use values outside the range of the image if you like, for example "-5, 150" is still valid. This center is used for the image employed when the pony is facing to the right.
* *Left Image Center* - As above, but this is the point used when the pony is facing to the left instead.
* *Prevent Animation Loop* - A Boolean flag ("True" or "False" - case sensitive). If the images being displayed are animated, the animations will loop according to how the image specifies this should occur. A GIF image for instance can specify an image loops endlessly, or provide an explicit number of times to loop. If this flag is set to False, this is what happens. If the flag is set to True then the image setting is ignored and the animation plays once and does not loop. Useful if want really want to ensure an animation that is not intended to loop really won't loop.
* *Group* - An integer between 0 and 100 inclusive. This specifies the behavior group this behavior belongs to, the default is 0. When a pony is selecting a new behavior to use at random, it will choose a behavior from the 0 group (known as the "Any" group) or the same group as the current behavior. This allows a pony to enter certain "modes" if desired. For example, you can create a group of behaviors where a pony is wearing a dress and put them all into group 1 and then create a set of behaviors without the dress and put them into group 2. This means if the pony is wearing a dress, it will only run other dress wearing behaviors. This means you can avoid flicking back and forth between dress and dressless behaviors which may look bad. Instead you can set up a transition behavior in each group, a "put-dress-on" behavior in group 2 which links to a behavior in group 1, and then a "take-dress-off" behavior in group 1 which links to a behavior in group 2. This will allow the pony to naturally transition between the two groups. The "Any" group (0) is special in that is it always considered when trying to select a new behavior, regardless of the current group. This can provide an alternative method for transitions. If you have some normal behaviors in the group, you can set up some dress behaviors in group 1 and some superhero behaviors in group 2. The pony will start out in the normal behaviors and then might run a behavior that transitions it into either the dress group or superhero group. Once in that group it might continue running something in that group, but might also choose a group 0 behavior and return to normal. However this prevents the pony switching between the dress and superhero groups. It would have to first choose something in the normal group before it could then have a chance of transitioning into the other group again.
* *Follow Offset Type* - This can be one of two case sensitive values, "Fixed" or "Mirror". The value affects how the offset coordinates are interpreted when the pony is following another pony. The coordinates are specified for when the target is facing to the right. For example, (-50, 0) would specify that a location 50 pixels to the left of the target should be sought out. If you need to always end up on the same side of the pony regardless on which way it is facing, use the "Fixed" value. This means in our example we will always seek out the point 50 pixels to the left of the target. Use the "Mirror" value to negate the X value when the target is facing left. In our example this means the offset becomes (50, 0) when our target is facing left, which means we will end up 50 pixels behind our target at all times (which means we could end up on either side, depending on which way it is facing).

### Effect

Should appear: 0 or more times.

#### Format
    Effect,*Effect Name*,*Behavior Name*,*Right Image*,*Left Image*,*Duration*,*Repeat Delay*,*Placement Right*,*Centering Right*,*Placement Left*,*Centering Left*,*Follow*,*Prevent Animation Loop*

#### Defaults
    Effect,*Effect Name Required*,*Behavior Name Required*,*Right Image Required*,*Left Image Required*,5,0,Any,Any,Any,Any,False,False

You MUST specify at least all the elements up to and including follow. You may rely on the defaults for subsequent elements and can leave them off if desired. The defaults for elements up to and including follow are the values Desktop Ponies will substitute if that element is invalid. These particular defaults are specific to Desktop Ponies only and are subject to change.

#### Examples
    Effect,"Apple Drop","gallop","apple_drop.gif","apple_drop.gif",3.3,0.5,Bottom_Left,Bottom,Bottom_Right,Bottom,False,False
    Effect,"crystalspark","crystallized","sparkle.gif","sparkle.gif",0,0,Center,Center,Center,Center,True,False

#### Elements
* *Effect Name* - A case insensitive name that uniquely identifies the effect for this pony. This may be quoted. If this name is not unique then it will still work but references to it from other areas may not work.
* *Behavior Name* - The name of a behavior belonging to the pony. When this behavior starts, this effect is activated.
* *Right Image* - The name of the image file to be used for the effect when the pony if facing to the right, which should be a file in the pony's directory. This should match the casing of the file so that the config can be ported between Windows and Unix systems without issues. If the file cannot be found the effect will be ignored.
* *Left Image* - As above, but for facing left.
* *Duration* - The number of seconds the effect should last. A decimal number between 0.0 and 300.0 inclusive. If the duration is zero, the effect will last for as long as the behavior that triggered it is still running and ends when that behavior ends.
* *Repeat Delay* - The number of seconds between repeats of the effect. A decimal number between 0.0 and 300.0 inclusive. If the repeat delay is zero, the effect does not repeat. Effects that do repeat will deploy a new image after the given time period whilst the behavior that triggered the effect is still running. For example, if you have an effect for dropping apples that repeats every second, attached to a behavior that lasts 5 seconds, then 5 apple dropping images will be deployed.
* *Placement Right* - Specifies where on the pony image the effect should be deployed from, when the pony is facing right. This can be one of a series of values. These values are case insensitive but it is recommended to use the exact casing in case other implementations rely on it.
    * Top_Left - The top left.
    * Top - The top center.
    * Top_Right - The top right.
    * Left - The middle left.
    * Center - The middle center.
    * Right - The middle right.
    * Bottom_Left - The bottom left.
    * Bottom - The bottom center.
    * Bottom_Right - The bottom right.
    * Any - One of the 9 possible points is chosen at random when the effect deploys.
    * Any-Not_Center - One of the 8 possible points (except the center) is chosen at random when the effect deploys.
* *Centering Right* - Specifies the center point of the effect image, when the pony is facing right. This can be one of the same values as the placement. When the effect is deployed the chosen center point of the effect image will be made to align with the chosen placement point on the pony image.
* *Placement Left* - Specifies where on the pony image the effect should be deployed from, when the pony is facing left.
* *Centering Left* - Specifies the center point of the effect image, when the pony is facing left.
* *Follow* - A Boolean flag ("True" or "False" - case sensitive). If False, when the effect is deployed it will remain stationary using the same facing as the pony had at the time. If True, the effect will follow the pony as it moves around (i.e. the chosen center point of the image will remain aligned with the chosen placement point on the pony) and its facing will be updated to match the pony as it switches directions.
* *Prevent Animation Loop* - A Boolean flag ("True" or "False" - case sensitive). The same as the flag for behaviors, this will override the image settings and prevent an animation looping if set to True.

### Speak

Should appear: 0 or more times.

#### Format
There are three accepted formats for speeches:

    Speak,*Speech Text*
    Speak,*Name*,*Speech Text*,*Sound File*,*Skip*,*Group*
    Speak,*Name*,*Speech Text*,*Sound Files*,*Skip*,*Group*

#### Defaults
    Speak,*Speech Text Required*
    Speak,*Name Required*,*Speech Text Required*,"",False,0
    Speak,*Name Required*,*Speech Text Required*,,False,0

The first format creates a speech with no name, the second supports a single sound file and the third supports a choice of sound files. Desktop Ponies supports all three formats but recommends only the third format is ever used for compatibility of configs with other programs. You MUST specify at least all the elements up to and including skip. You may rely on the defaults for subsequent elements and can leave them off if desired. The defaults for elements up to and including skip are the values Desktop Ponies will substitute if that element is invalid. These particular defaults are specific to Desktop Ponies only and are subject to change.
	
#### Examples
    Speak,"Hi"
    Speak,"Howdy","Howdy, Partner!",,False,0
    Speak,"cock-a-doodle","Cock-a-doodle-doo!",,True,1
    Speak,"Yeehaw","Yee-haw!","yeehaw.mp3",False,0
    Speak,"Yeehaw","Yee-haw!",{"yeehaw.mp3","yeehaw.ogg"},False,0

#### Elements
* *Name* - A case insensitive name that uniquely identifies the speech for this pony. This may be quoted. If this name is not unique then it will still work but references to it from other areas may not work. For example, if a behavior refers to a start or end speech these may fail if the name is not unique.
* *Speech Text* - A text of the speech that should be shown when the pony speaks. This may be quoted.
* *Sound File* - An optional sound file that should be played with the speech. If this is blank then no file should be played. Otherwise is will be a single optionally quoted path to a sound file. See the Sound Files property for full details on how sounds are handled.
* *Sound Files* - An optional sound file that should be played with the speech. If this is blank then no file should be played. Otherwise it will be a list of quoted, comma separated paths inside a set of braces. These paths should be files in the pony's directory. This should match the casing of the file so that the config can be ported between Windows and Unix systems without issues. The motivation for providing a list of files was for compatibility with Browser Ponies which required the .ogg format as opposed to the .mp3 format. Browser Ponies will look for the .ogg file and Desktop Ponies will look for the .mp3 file. It is extremely highly recommended to only ever use a list containing these two files, listed in order with a matching name; i.e. `{"*name*.mp3","*name*.ogg"}`. Swapping the order, using files with different names or listing less or more files will probably cause issues if you intend to use the config with programs other the Desktop Ponies. Additionally, Desktop Ponies will only ever attempt to save a list of sound files in this format so if you enter extra names they will be lost. If you are the author of another program and require another format, it is suggested you simply use the name for the .mp3 file and substitute you own extension, creating your version of the sound files with matching filenames.
* *Skip* - A Boolean flag ("True" or "False" - case sensitive). If False, that speech can be selected to be used at random and may be played occasionally when the pony switches behaviors if it has no other speech to say. If True, this prevents the speech being selected for use at random. This is useful if you need to prevent certain speeches being used at random (e.g. if the speech is only intended to be used as the start or end speech for a specific behavior).
* *Group* - An integer between 0 and 100 inclusive. This specifies the behavior group this speech belongs to, the default is 0. A speech can only be selected to be played at random if the group is 0 (part of the Any group) or the group number of the current behavior matches the group number of the speech. For example, you could create some Gala speeches and put them into group 1, if you also had a set of behaviors for a pony wearing a gala dress that were part of group 1. These speeches could be played whilst the pony was wearing the dress, but not at other times.

### Scale

Should appear: 0 or 1 times.

#### Format
    Scale,*ScaleFactor*

#### Examples
    Scale,1.5
    Scale,0.6

This type of line is deprecated and no longer has any effect. It will be ignored.

### Interaction

Should appear: 0 or more times.

#### Format
    Interaction,*Name*,*Chance*,*Proximity*,*Targets*,*Target Activation*,*Behaviors*,*Reactivation Delay*

#### Defaults
    Interaction,"",0,125,*Targets Required*,One,*Behaviors Required*,60

You MUST specify at least all the elements up to and including behaviors. You may rely on the defaults for subsequent elements and can leave them off if desired. The defaults for elements up to and including behaviors are the values Desktop Ponies will substitute if that element is invalid. These particular defaults are specific to Desktop Ponies only and are subject to change.

#### Examples
    Interaction,nervous,1,100,{"Braeburn"},One,{"nervous"},30
    Interaction,Conga,0.2,250,{"Applejack","Fluttershy","Rainbow Dash","Rarity","Twilight Sparkle"},All,{"Conga Start"},300
	Interaction,random,0.5,150,{"Pinkie Pie","Rainbow Dash"},Any,{"Random1","Random2"},3600

#### Elements
* *Name* - A case insensitive name that uniquely identifies the interaction. This may be quoted. If this name is not unique then it will still work but references to it from other areas may not work.
* *Chance* - A decimal number between 0.0 and 1.0 inclusive. Whilst an interaction is in a state where it could be started then this chance will be spun against a dice roll each tick (a tick is 1/25 of a second currently) to see whether it should start. A chance of 1.0 means it will definitely start if possible, whereas a chance of 0.0 means it never will. This pony must be present for the interaction to be considered for activation. If this pony is busy then the interaction cannot run until it stops being busy. A pony that is busy is unavailable for interaction. A pony is considered busy if it is already involved in an interaction, is being moused-over or dragged, is sleeping, or is being manually directed by the program for various reasons (this includes ponies under manual control by the user, or ponies outside the allowed region of the screen that are being directed to return to the allowed area). If a pony becomes busy whilst they are involved in an interaction (e.g. they start being dragged), the interaction will be canceled and this pony and all targets will resume with random behaviors.
* *Proximity* - An integer between 0 and 10000 inclusive that specifies the distance is pixels at which the interaction is considered for activation. If the distance between any potential target and this pony is less than this value then the interaction can be used. This prevents interactions starting until the targets are within a certain range of each other.
* *Targets* - A unordered, quoted, comma separated list surrounded by braces that gives a list of targets (ponies) that should be involved in the interaction. How the targets are used is affected by the target activation element. At least one of these targets must be present and within range of this pony for the interaction to be considered for activation. You may have a target that is the same as this pony, in which case this pony will interact with a second instance of itself. You cannot however duplicate targets. For example, if you list "Rarity" twice in the list, the second one will be ignored.
* *Target Activation* - Controls how many targets are required for the interaction to be considered for activation. This can be one of a series of values. These values are case sensitive.
    * One - Specifies that the interaction acts with only one of the listed targets. The target that comes within range is that one that triggers the interaction, and the one that this pony interacts with. If both this pony and this target are not busy and possess a behavior capable of being run, they begin the interaction. (Desktop Ponies also supports the case insensitive values "False" and "random" for this setting, which were used in older versions of this format).
    * Any - Specifies that the interaction will start with any targets that are present if one of them triggers the interaction. For example if an interaction has targets of Rarity, Fluttershy and Applejack and only Rarity and Fluttershy are onscreen, then the interaction can start if Rarity walks within the trigger distance. Fluttershy does not need to be within the trigger distance and Applejack not being present does not matter. Only if this pony and trigger are not busy and both possess a behavior capable of being run can the interaction start. Additional targets will only become involved if they are not busy and also possess behaviors capable of being run. If there are other instances of the trigger pony onscreen, the trigger becomes involved in the interaction in preference to the others that did not trigger it. (Desktop Ponies also supports the case insensitive values "True" and "all" for this setting, which were used in older versions of this format).
    * All - Specifies that the interaction requires all the listed targets to be present. This means that at least one instance of each target must be onscreen, not busy and have a behavior capable of being run, as well as this pony. Once any one of the targets walks within the trigger distance the interaction can start. If there are other instances of the trigger pony onscreen, the trigger becomes involved in the interaction in preference to the others that did not trigger it.
* *Behaviors* - An unordered, quoted, comma separated list surrounded by braces that gives a list of behaviors. Typically only one behavior is ever specified. When an interaction is being considered each target will individually select a behavior uniformly at random from this list, provided the behavior is in the 'Any' group or the current behavior group for the pony. If no such behavior exists because the pony lacks behaviors with any of the listed names, or the behaviors are part of the wrong group, then the pony cannot participate because it does not possess a behavior capable of being run.
* *Reactivation Delay* - A decimal number between 0.0 and 3600.0 inclusive. Specifies a time period in seconds. After an interaction finishes, all ponies involved in the interaction will undergo a cooldown period where they cannot initiate any interactions until it expires. They may however still be considered as targets for interactions during this time. If an interaction is canceled rather than ending normally, the cooldown period is capped at 30 seconds. This is because it may not have completed, so it would be nice to allow the ponies to start interacting earlier and try again.

## interactions.ini File Format

The file named "interactions.ini" (always lowercase) inside the Ponies directory specifies interactions between ponies. The general format matches that of the pony.ini format.

It supports only a single type of line for configuring interactions. This is a legacy format that will be upgraded by Desktop Ponies automatically on startup. It lacks the line identifer and contains an extra element.

#### Format
    *Name*,*Initiator*,*Chance*,*Proximity*,*Targets*,*Target Activation*,*Behaviors*,*Reactivation Delay*

#### Defaults
    "",*Initiator Required*,0,125,*Targets Required*,One,*Behaviors Required*,60

#### Examples
    nervous,"Little Strongheart",1,100,{"Braeburn"},One,{"nervous"},30
    Conga,"Pinkie Pie",0.2,250,{"Applejack","Fluttershy","Rainbow Dash","Rarity","Twilight Sparkle"},All,{"Conga Start"},300
	random,Pinkie Pie,0.5,150,{"Pinkie Pie","Rainbow Dash"},Any,{"Random1","Random2"},3600

#### Elements
As the standard interaction and also:
* *Initiator* - The name of a pony that initiates the interaction. This may be quoted.
