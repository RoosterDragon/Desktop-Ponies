Imports DesktopSprites.SpriteManagement

Public Interface IExpireableSprite
    Inherits ISprite
    Sub Expire()
    Event Expired As EventHandler
End Interface