using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBattle
{
    interface IVisibleObj
    {
        public  void Render(Graphics g);
        public  void UpdateFrame(int tickCount);
    }
    interface IWeapon
    {
        public  void Attack();

        public  bool CheckColliding(Bullet bullet);

    }
    abstract class GameObj : IVisibleObj
    {
        public Point Position;
        public virtual void Render(Graphics g)
        {
        }
        public virtual void UpdateFrame(int tickCount)
        {
        }
    }
    abstract class MovableObj : GameObj
    {
        protected Point ptStart;
        protected double direction;
        protected int speed;
        private Point EndPoint { set; get; }
        public MovableObj()
        {
            EndPoint = new Point(-1000, -1000);
        }
        public Size GetMoveSpace()
        {
            return new Size((int)(speed * Math.Cos(direction)),
                (int)(speed * Math.Sin(direction)));
        }
        public override void UpdateFrame(int tickCount)
        {
            if (EndPoint.X != -1000 && EndPoint.Y != -1000)
            {
                if (Math.Abs(EndPoint.X - Position.X) < speed
                    || Math.Abs(EndPoint.Y - Position.Y) < speed)
                {
                    Position = EndPoint;
                    return;
                }
            }
            Position.Offset((int)(speed * Math.Cos(direction)),
                (int)(speed * Math.Sin(direction)));
        }
    }
    abstract class VisiableObj : GameObj
    {
        private Bitmap pic;
        public String Pic { set { pic = new Bitmap(value); } }

        public Rectangle Rect
        {
            get
            {
                return new Rectangle(Position, pic.Size);
            }
        }
        public override void Render(Graphics g)
        {
            if (pic != null)
                g.DrawImage(pic, Position.X, Position.Y, pic.Width, pic.Height);
        }
    }


}
