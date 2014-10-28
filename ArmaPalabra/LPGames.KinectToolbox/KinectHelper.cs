using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;

namespace LPGames.KinectToolbox
{
    public class KinectHelper
    {
        public static Skeleton GetPrimarySkeleton(IEnumerable<Skeleton> skeletons)
        {
            Skeleton primarySkeleton = null;
            foreach (Skeleton skeleton in skeletons)
            {
                if (skeleton.TrackingState != SkeletonTrackingState.Tracked)
                {
                    continue;
                }

                if (primarySkeleton == null)
                    primarySkeleton = skeleton;
                else if (primarySkeleton.Position.Z > skeleton.Position.Z)
                    primarySkeleton = skeleton;
            }
            return primarySkeleton;
        }

        public static Vector2 GetVectorScale(Vector2 jointVector,int width, int height)
        {
            Vector2 position = new Vector2(KinectScaler.Scale(width, 0.5f, jointVector.X), KinectScaler.Scale(height, 0.5f, -1*jointVector.Y));

            return position;

        }
    }
}
