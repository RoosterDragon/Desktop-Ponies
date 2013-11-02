Imports DesktopSprites.SpriteManagement

''' <summary>
''' Defines a sprite which may be dragged.
''' </summary>
Public Interface IDraggableSprite
    Inherits ISprite
    ''' <summary>
    ''' Gets or sets whether the sprite should be in a state where it acts as if dragged by the cursor.
    ''' </summary>
    Property Drag As Boolean
End Interface
