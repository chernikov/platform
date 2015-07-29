using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace platformAthletic.Model
{ 
    public partial class SBCValue
    {
        public static SBCValue EmptySBCValue = new SBCValue();

        public enum SbcType
        {
            Squat = 0x01,
            Bench = 0x02, 
            Clean = 0x03
        }

        public override bool Equals(object obj)
        {
            var sbcOrigin = obj as SBCValue;
            if (sbcOrigin != null) 
            {
                return sbcOrigin.Clean == Clean && sbcOrigin.Bench == Bench && sbcOrigin.Squat == Squat;
            }
            return false;
        }
	}
}