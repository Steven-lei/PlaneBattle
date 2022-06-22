using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaneBattle
{
    internal class Bullet : GameObj
    {
        private double direction;
        protected Point ptStart;
        protected int speed;
        private Bitmap pic;
        public String Pic { set { pic = new Bitmap(value); } }

        public Rectangle AttackRect
        {
            get
            {
                Point pt = Position;
                pt.Offset(-pic.Width/2,-pic.Height/2);
                return new Rectangle(pt, pic.Size);
            }
        }
        private int createTickCount = Environment.TickCount;
        public Bullet(Point myPos, Point posAttack, int speed)
        {
            Position = myPos;
            this.direction = Math.Atan2(((double)posAttack.Y - myPos.Y) , (posAttack.X - myPos.X));
 //           System.Diagnostics.Debug.WriteLine(this.direction/Math.PI * 180);
            this.ptStart = myPos;
            this.speed = speed;
            Pic = "images//bullet1.png";
        }
        public override void Render(Graphics g)
        {
            if (pic != null)
                g.DrawImage(pic, Position.X - pic.Width/2, Position.Y-pic.Height/2, pic.Width, pic.Height);
        }
        public override void UpdateFrame(int tickCount)
        {
            int dis = (tickCount - createTickCount) * speed / 10;
            Position = ptStart;
            Position.Offset((int)(dis * Math.Cos(direction)),
                (int)(dis * Math.Sin(direction)));
        }
    }
}
