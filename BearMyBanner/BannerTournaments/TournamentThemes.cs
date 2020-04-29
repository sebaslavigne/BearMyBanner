using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearMyBanner.Wrapper
{
    public class TournamentThemes
    {

        public TournamentThemes(Culture culture)
        {
            AvailableThemes.AddRange(_generic);
            switch (culture)
            {
                case Culture.Aserai:
                    AvailableThemes.AddRange(_aserai);
                    break;
                case Culture.Battania:
                    AvailableThemes.AddRange(_battania);
                    break;
                case Culture.Empire:
                    AvailableThemes.AddRange(_empire);
                    break;
                case Culture.Khuzait:
                    AvailableThemes.AddRange(_khuzait);
                    break;
                case Culture.Sturgia:
                    AvailableThemes.AddRange(_sturgia);
                    break;
                case Culture.Vlandia:
                    AvailableThemes.AddRange(_vlandia);
                    break;
                default:
                    break;
            }
        }

        public List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)> AvailableThemes = new List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)>();

        public  readonly int[] PrimaryColors = new int[] { 119, 118, 120, 121 };
        public readonly (int white, int black) SecondaryColors = (128, 149);

        private readonly List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)> _generic = new List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)>
        {
            (true, false, false, new int[] { 11, 11, 11, 11 }), // plain
            (true, false, true, new int[] { 1, 3, 12, 14 }), // two tone A
            (true, false, true, new int[] { 12, 13, 14, 16 }), // two tone B
            (true, false, false, new int[] { 23, 24, 27, 28 }), // lines
            (true, false, true, new int[] { 31, 32, 33, 34 }) // base shapes
        };

        private readonly List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)> _aserai = new List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)>
        {
            (false, true, true, new int[] { 425, 426, 427, 428 }), // moons
            (false, true, false, new int[] { 118, 118, 118, 118 }), // Aserai
            (false, true, true, new int[] { 223, 223, 223, 223 }) // sarran
                
        };
        private readonly List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)> _battania = new List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)>
        {
            (false, true, true, new int[] { 523, 525, 526, 534 }), // runes A
            (false, true, false, new int[] { 530, 531, 532, 533 }) // runes B
        };
        private readonly List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)> _empire = new List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)>
        {
            (false, true, false, new int[] { 202, 202, 202, 202 }), // imperial A
            (false, true, false, new int[] { 423, 423, 423, 423 }), // imperial B
            (true, true, true, new int[] { 217, 218, 219, 220 }), // big flora
            (false, true, false, new int[] { 340, 340, 340, 340 }) // crown
        };
        private readonly List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)> _khuzait = new List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)>
        {
            (false, true, true, new int[] { 425, 426, 427, 428 }) // moons
        };
        private readonly List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)> _sturgia = new List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)>
        {
            (false, true, true, new int[] { 523, 525, 526, 534 }), // runes A
            (false, true, false, new int[] { 530, 531, 532, 533 }) // runes B
        };
        private readonly List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)> _vlandia = new List<(bool mirrorable, bool isIcon, bool swappable, int[] meshes)>
        {
            (true, true, false, new int[] { 202, 205, 206, 207 }), // small flora
            (true, true, true, new int[] { 217, 218, 219, 220 }), // big flora
            (true, true, true, new int[] { 512, 513, 514, 221 }) // poker
        };
    }
}
