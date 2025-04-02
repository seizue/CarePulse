using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;

namespace CarePulse
{
    public partial class CustomDatePicker : DateTimePicker
    {
        private Color fillColor = Color.LightSeaGreen;
        private Color textColor = Color.White;
        private Color borderColor = Color.Gray;
        private int borderSize = 0;
        private Image CalenderImg = Properties.Resources.calendar_24px;
        private RectangleF iconButton;
        private const int iconWidth = 42;
        private const int arrowWidth = 17;
        //private bool dropDown = true;

        // New property to control icon-only mode
        private bool showIconOnly = false;
        public bool ShowIconOnly
        {
            get { return showIconOnly; }
            set
            {
                showIconOnly = value;
                this.Invalidate();
            }
        }

        public Color FillColor
        {
            get { return fillColor; }
            set
            {
                fillColor = value;
                if (fillColor.GetBrightness() >= 0.7f)
                {
                    CalenderImg = Properties.Resources.calendar_24px;
                }
                else
                {
                    CalenderImg = Properties.Resources.calendar_24px2;
                }
                this.Invalidate();
            }
        }

        public Color TextColor
        {
            get { return textColor; }
            set
            {
                textColor = value;
                this.Invalidate();
            }
        }

        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate();
            }
        }

        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Invalidate();
            }
        }


        public CustomDatePicker()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.MinimumSize = new Size(0, 35);
            this.Font = new Font(this.Font.Name, 9.5f);
        }

        protected override void OnDropDown(EventArgs eventargs)
        {
            base.OnDropDown(eventargs);
            //dropDown = true;
        }

        protected override void OnCloseUp(EventArgs eventargs)
        {
            base.OnCloseUp(eventargs);
            //dropDown = false;
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            e.Handled = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (Graphics graphics = this.CreateGraphics())
            using (Pen pen = new Pen(borderColor, borderSize))
            using (SolidBrush fillBrush = new SolidBrush(fillColor))
            using (SolidBrush textBrush = new SolidBrush(textColor))
            using (StringFormat format = new StringFormat())
            {
                RectangleF dtpArea = new RectangleF(0, 0, this.Width - 0.5f, this.Height - 0.5f);
                RectangleF iconArea = new RectangleF(dtpArea.Width - iconWidth, 0, iconWidth, dtpArea.Height);
                pen.Alignment = PenAlignment.Inset;
                format.LineAlignment = StringAlignment.Center;

                graphics.FillRectangle(fillBrush, dtpArea);

                if (!showIconOnly)
                {
                    graphics.DrawString("    " + this.Text, this.Font, textBrush, dtpArea, format);
                }

                if (borderSize >= 1) graphics.DrawRectangle(pen, dtpArea.X, dtpArea.Y, dtpArea.Width, dtpArea.Height);

                // Calculate icon position
                int iconX = showIconOnly ? (this.Width - CalenderImg.Width) / 2 : this.Width - CalenderImg.Width - 9;
                int iconY = (this.Height - CalenderImg.Height) / 2;
                graphics.DrawImage(CalenderImg, iconX, iconY);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            int iconWidth = GetIconWidth();
            iconButton = new RectangleF(this.Width - iconWidth, 0, iconWidth, this.Height);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (iconButton.Contains(e.Location))
                this.Cursor = Cursors.Hand;
            else this.Cursor = Cursors.Default;
        }

        private int GetIconWidth()
        {
            int textWidth = TextRenderer.MeasureText(this.Text, this.Font).Width;
            if (textWidth <= this.Width - (iconWidth + 20))
                return iconWidth;
            else return arrowWidth;
        }
    }
}
