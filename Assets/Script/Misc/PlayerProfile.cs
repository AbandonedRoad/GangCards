using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Misc
{
    [Serializable]
    public class PlayerProfile
    {
        public bool AAIsActive { get; set; }
        public string ActiveProfile { get; set; }
        public Dictionary<int, string> Profile { get; set; }
        public bool MusicIsActive { get; set; }
        public bool SFXsActive { get; set; }
        public bool SSAOIsActive { get; set; }

        /// <summary>
        /// The Player Profile
        /// </summary>
        public PlayerProfile()
        {
            Profile = new Dictionary<int, string>();

            for (int i = 0; i < 5; i++)
            {
                Profile.Add(i, String.Empty);
            }
        }
    }
}
