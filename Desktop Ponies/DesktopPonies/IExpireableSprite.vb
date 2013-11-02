Imports DesktopSprites.SpriteManagement

''' <summary>
''' Defines a sprite which can expire, which indicates it should no longer be displayed nor affect other sprites.
''' </summary>
Public Interface IExpireableSprite
    Inherits ISprite
    ''' <summary>
    ''' Manually expires the sprite, which triggers the Expired event.
    ''' </summary>
    Sub Expire()
    ''' <summary>
    ''' Occurs when the sprite expires.
    ''' </summary>
    Event Expired As EventHandler
End Interface