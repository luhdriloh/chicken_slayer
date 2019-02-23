using UnityEngine;

public class LaserPointerPixel : MonoBehaviour
{
    public Texture2D _texture;
    public Transform _laserTip;
    public int _pixelSize;
    public int _guiDepth;
    public LayerMask _border;

    // maybe make the texture static and make all beams use this
    private Texture2D _beamTexture;
    private Rect _screenRect;
    private Color32[] _clearTexture;

    private ContactFilter2D _filter;
    private Vector2 _directionVector;


    private void Start()
    {
        _screenRect = new Rect(Vector2.zero, new Vector2(Screen.width, Screen.height));
        _beamTexture = new Texture2D((int)_screenRect.width, (int)_screenRect.height, TextureFormat.RGBA32, false)
        {
            wrapMode = TextureWrapMode.Clamp
        };

        SetClearTexture(_beamTexture);
    }

    private void OnPreRender()
    {
        _beamTexture.SetPixels32(_clearTexture);
        Vector2 laserTipScreenPosition = Camera.main.WorldToScreenPoint(_laserTip.position);

        // the colliders hit will be stored in hits
        RaycastHit2D hit = Physics2D.Raycast(_laserTip.position, _laserTip.right, distance: Mathf.Infinity, layerMask: _border);
        Vector2 point = hit.collider != null ? hit.point : (Vector2)_laserTip.position;
        DrawLine(_beamTexture, laserTipScreenPosition, Camera.main.WorldToScreenPoint(point), Color.red);
        _beamTexture.Apply();
    }

    private void OnPostRender()
    {
        GL.PushMatrix();
        GL.LoadPixelMatrix(0, _screenRect.width, 0, _screenRect.height);

        Graphics.DrawTexture(_screenRect, _beamTexture);
        GL.PopMatrix();
    }

    //private void Update()
    //{
    //    _beamTexture.SetPixels32(_clearTexture);
    //    Vector2 laserTipScreenPosition = Camera.main.WorldToScreenPoint(_laserTip.position);

    //    // the colliders hit will be stored in hits
    //    RaycastHit2D hit = Physics2D.Raycast(_laserTip.position, _laserTip.right, distance: Mathf.Infinity, layerMask: _border);
    //    Vector2 point;
    //    if (hit.collider != null)
    //    {
    //        point = hit.point;
    //    }
    //    else
    //    {
    //        point = _laserTip.position;
    //    }

    //    DrawLine(_beamTexture, laserTipScreenPosition, Camera.main.WorldToScreenPoint(point), Color.red);
    //    _beamTexture.Apply();

    //    Graphics.DrawTexture(_screenRect, _beamTexture);
    //}

    private void SetClearTexture(Texture2D tex)
    {
        _clearTexture = tex.GetPixels32();
        Color32 clear = new Color32(0, 0, 0, 0);

        for (int i = 0; i < _clearTexture.Length; i++)
        {
            _clearTexture[i] = clear;
        }
    }

    // code so that if the laser is out of bounds it automatically get the fuck out

    private void DrawLine(Texture2D tex, Vector2 initial, Vector2 end, Color col)
    {
        int x0 = (int)initial.x;
        int y0 = (int)initial.y;

        int x1 = (int)end.x;
        int y1 = (int)end.y;

        int xStart = x0;
        int yStart = y0;

        int dy = (int)(y1 - y0);
        int dx = (int)(x1 - x0);
        int stepx, stepy;

        if (dy < 0) { dy = -dy; stepy = -1; }
        else { stepy = 1; }
        if (dx < 0) { dx = -dx; stepx = -1; }
        else { stepx = 1; }
        dy <<= 1;
        dx <<= 1;

        float fraction = 0;

        tex.SetPixel(x0, y0, col);
        if (dx > dy)
        {
            fraction = dy - (dx >> 1);

            while (Mathf.Abs((xStart + (x0 - xStart) * _pixelSize) - x1) > _pixelSize)
            {
                if (fraction >= 0)
                {
                    y0 += stepy;
                    fraction -= dx;
                }
                x0 += stepx;
                fraction += dy;
                
                //tex.SetPixel(x0, y0, col);
                SetPixelToSetSize(x0, y0, xStart, yStart, tex, col);
            }
        }
        else
        {
            fraction = dx - (dy >> 1);
            while (Mathf.Abs((yStart + (y0 - yStart) * _pixelSize) - y1) > _pixelSize)
            {
                if (fraction >= 0)
                {
                    x0 += stepx;
                    fraction -= dy;
                }
                y0 += stepy;
                fraction += dx;
                //tex.SetPixel(x0, y0, col);
                SetPixelToSetSize(x0, y0, xStart, yStart, tex, col);
            }
        }
    }


    private void SetPixelToSetSize(int x, int y, int xStart, int yStart, Texture2D texture, Color color)
    {
        int xStartPosition = xStart + (x - xStart) * _pixelSize;
        int yStartPosition = yStart + (y - yStart) * _pixelSize;

        for (int i = xStartPosition; i < xStartPosition + _pixelSize; i++)
        {
            for (int j = yStartPosition; j < yStartPosition + _pixelSize; j++)
            {
                texture.SetPixel(i, j, color);
            }
        }
    }
}
