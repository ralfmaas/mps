﻿using Microsoft.Xna.Framework;
using System.Drawing;

public class XNA2dCamera : GraphicsDeviceManager
{
    #region Properties and Fields
    #region Position

    protected Vector2 position = Vector2.Zero;
    public Vector2 Position
    {
        get { return position; }
        set
        {
            position = value;
            visibleArea.X = position.X + offset.X - visibleArea.Width / 2;
            visibleArea.Y = position.Y + offset.Y - visibleArea.Height / 2;
        }
    }

    protected Vector2 offset = Vector2.Zero;
    public Vector2 Offset
    {
        get { return offset; }
        set
        {
            offset = value;
            visibleArea.X = position.X + offset.X - visibleArea.Width / 2;
            visibleArea.Y = position.Y + offset.Y - visibleArea.Height / 2;
        }
    }
    #endregion Position

    #region Culling
    // RectangleF = a Rectangle class that uses floats instead of ints
    protected RectangleF visibleArea;
    public RectangleF VisibleArea
    {
        get { return visibleArea; }
    }

    public float ViewingWidth
    {
        get { return visibleArea.Width; }
        set { visibleArea.Width = value; }
    }

        public float ViewingHeight
    {
        get { return visibleArea.Height; }
        set { visibleArea.Height = value; }
    }
    #endregion Culling

    #region Transformations
    protected float rotation;
    public float Rotation
    {
        get { return rotation; }
        set { rotation = value; }
    }

    // <0 - <1 = Zoom Out
    // >1 = Zoom In
    // <0 = Funky (flips axis)
    protected Vector2 zoom = Vector2.One;
    public Vector2 Zoom
    {
        get { return zoom; }
        set
        {
            // Maximale en minimale zoom
            if (value.X >= 0.2 && value.X <= 1)
                zoom = value;
        }
    }
    #endregion Transformations

    public Vector2 ScreenPosition
    {
        get { return new Vector2(graphicsManager.GraphicsDevice.Viewport.Width / 2, graphicsManager.GraphicsDevice.Viewport.Height / 2); }
    }

    GraphicsDeviceManager graphicsManager;
    #endregion Properties and Fields

    #region Constructors
    public XNA2dCamera(GraphicsDeviceManager graphics)
        : base(new Game())
    {
        graphicsManager = graphics;
        visibleArea = new RectangleF(0, 0, graphicsManager.GraphicsDevice.Viewport.Width, graphicsManager.GraphicsDevice.Viewport.Height);
        position = ScreenPosition;
    }
    public XNA2dCamera(float width, float height, Game game)
        : base(game)
    {
        visibleArea = new RectangleF(0, 0, width, height);
        position = ScreenPosition;
    }
    public XNA2dCamera(float x, float y, float width, float height, Game game)
        : base(game)
    {
        visibleArea = new RectangleF(x - (width / 2), y - (height / 2), width, height);
        position.X = x;
        position.Y = y;
    }
    #endregion Constructors

    #region Methods
    ///
    /// Returns a transformation matrix based on the camera’s position, rotation, and zoom.
    /// Best used as a parameter for the SpriteBatch.Begin() call.
    ///
    public virtual Matrix ViewTransformationMatrix()
    {
        Vector3 matrixRotOrigin = new Vector3(Position + Offset, 0);
        Vector3 matrixScreenPos = new Vector3(ScreenPosition, 0.0f);

        // Translate back to the origin based on the camera’s offset position, since we’re rotating around the camera
        // Then, we scale and rotate around the origin
        // Finally, we translate to SCREEN coordinates, so translation is based on the ScreenCenter
        return Matrix.CreateTranslation(-matrixRotOrigin) *
            Matrix.CreateScale(Zoom.X, Zoom.Y, 1.0f) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateTranslation(matrixScreenPos);
    }
    #endregion Methods
}