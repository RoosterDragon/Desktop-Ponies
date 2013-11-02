Imports DesktopSprites.SpriteManagement

''' <summary>
''' Defines a sprite which can play sounds.
''' </summary>
Public Interface ISoundfulSprite
    Inherits ISprite
    ''' <summary>
    ''' Gets the path to the sound file that should be played starting as of this update, or null to indicate nothing new should be
    ''' started.
    ''' </summary>
    ReadOnly Property SoundPath As String
End Interface
