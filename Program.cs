using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using aviationLib;

namespace navLoader
{
    class Program
    {
        static Morse morse = new Morse();

        static Char[] recordType_001_04 = new Char[04];
        static Char[] facilityId_005_04 = new Char[04];
        static Char[] type_009_20 = new Char[20];
        static Char[] freq_534_06 = new Char[06];
        static Char[] tacan_530_04 = new Char[04];
        static Char[] magVar_480_05 = new Char[05];
        static Char[] magVarYear_485_04 = new Char[04];
        static Char[] radioCall_500_30 = new Char[30];
        static Char[] fssId_616_04 = new Char[04];
        static Char[] fssName_620_30 = new Char[30];
        static Char[] name_043_30 = new Char[30];
        static Char[] city_073_40 = new Char[40];
        static Char[] state_143_02 = new Char[02];
        static Char[] class_282_11 = new Char[11];
        static Char[] artccHighId_304_04 = new Char[04];
        static Char[] artccHighName_308_30 = new Char[30];
        static Char[] artccLowId_338_04 = new Char[04];
        static Char[] artccLowName_342_30 = new Char[30];
        static Char[] latitude_372_14 = new Char[14];
        static Char[] longitude_397_14 = new Char[14];
        static Char[] status_770_30 = new Char[30];
        static Char[] hiwas_804_01 = new Char[01];
        static Char[] elevation_473_07 = new Char[07];

        static Char[] remarks_029_600 = new Char[600];

        static StreamWriter ofileNAV1 = new StreamWriter("navNavaid.txt");
        static StreamWriter ofileNAV2 = new StreamWriter("navRemarks.txt");

        static void Main(String[] args)
        {
            String userprofileFolder = Environment.GetEnvironmentVariable("USERPROFILE");
            String[] fileEntries = Directory.GetFiles(userprofileFolder + "\\Downloads\\", "28DaySubscription*.zip");

            ZipArchive archive = ZipFile.OpenRead(fileEntries[0]);
            ZipArchiveEntry entry = archive.GetEntry("NAV.txt");
            entry.ExtractToFile("NAV.txt", true);

            StreamReader file = new StreamReader("NAV.txt");

            String rec = file.ReadLine();

            while (!file.EndOfStream)
            {
                ProcessRecord(rec);

                rec = file.ReadLine();
            }

            ProcessRecord(rec);

            file.Close();

            ofileNAV1.Close();
            ofileNAV2.Close();
        }

        static void ProcessRecord(String record)
        {
            recordType_001_04 = record.ToCharArray(0, 4);

            String rt = new String(recordType_001_04);
            Int32 r = String.Compare(rt, "NAV1");
            if (r == 0)
            {
                facilityId_005_04 = record.ToCharArray(4, 4);
                String s = new String(facilityId_005_04).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                Morse.LetterCode lc = morse.list.Find(x => x.letter.Contains(facilityId_005_04[0].ToString()));
                if (lc.code != null)
                {
                    ofileNAV1.Write(lc.code);
                }

                lc = morse.list.Find(x => x.letter.Contains(facilityId_005_04[1].ToString()));
                if (lc.code != null)
                {
                    ofileNAV1.Write(' ');
                    ofileNAV1.Write(lc.code);
                }

                lc = morse.list.Find(x => x.letter.Contains(facilityId_005_04[2].ToString()));
                if (lc.code != null)
                {
                    ofileNAV1.Write(' ');
                    ofileNAV1.Write(lc.code);
                }

                lc = morse.list.Find(x => x.letter.Contains(facilityId_005_04[3].ToString()));
                if (lc.code != null)
                {
                    ofileNAV1.Write(' ');
                    ofileNAV1.Write(lc.code);
                }

                ofileNAV1.Write('~');

                type_009_20 = record.ToCharArray(8, 20);
                s = new String(type_009_20).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                freq_534_06 = record.ToCharArray(533, 6);
                s = new String(freq_534_06).Trim();
                if(s.Length != 3)
                {
                    s = s.PadRight(7, '0');
                }
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                tacan_530_04 = record.ToCharArray(529, 4);
                s = new String(tacan_530_04).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                magVar_480_05 = record.ToCharArray(479, 5);
                s = new String(magVar_480_05).Trim();
                MagVar mv = new MagVar(s);
                ofileNAV1.Write(mv.magVar.ToString("F2"));
                ofileNAV1.Write('~');

                magVarYear_485_04 = record.ToCharArray(484, 4);
                s = new String(magVarYear_485_04).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                radioCall_500_30 = record.ToCharArray(499, 30);
                s = new String(radioCall_500_30).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                fssId_616_04 = record.ToCharArray(615, 4);
                s = new String(fssId_616_04).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                fssName_620_30 = record.ToCharArray(619, 30);
                s = new String(fssName_620_30).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                name_043_30 = record.ToCharArray(42, 30);
                s = new String(name_043_30).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                city_073_40 = record.ToCharArray(72, 40);
                s = new String(city_073_40).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                state_143_02 = record.ToCharArray(142, 2);
                s = new String(state_143_02).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                class_282_11 = record.ToCharArray(281, 11);
                s = new String(class_282_11).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                artccHighId_304_04 = record.ToCharArray(303, 4);
                s = new String(artccHighId_304_04).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                artccHighName_308_30 = record.ToCharArray(307, 30);
                s = new String(artccHighName_308_30).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                artccLowId_338_04 = record.ToCharArray(337, 4);
                s = new String(artccLowId_338_04).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                artccLowName_342_30 = record.ToCharArray(341, 30);
                s = new String(artccLowName_342_30).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                latitude_372_14 = record.ToCharArray(371, 14);
                longitude_397_14 = record.ToCharArray(396, 14);

                LatLon ll = new LatLon(new String(latitude_372_14).Trim(), new String(longitude_397_14).Trim());
                ofileNAV1.Write(ll.formattedLat);
                ofileNAV1.Write('~');

                ofileNAV1.Write(ll.formattedLon);
                ofileNAV1.Write('~');

                status_770_30 = record.ToCharArray(769, 30);
                s = new String(status_770_30).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                hiwas_804_01 = record.ToCharArray(803, 1);
                s = new String(hiwas_804_01).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write('~');

                elevation_473_07 = record.ToCharArray(472, 7);
                s = new String(elevation_473_07).Trim();
                ofileNAV1.Write(s);
                ofileNAV1.Write(ofileNAV1.NewLine);
            }

            rt = new String(recordType_001_04);
            r = String.Compare(rt, "NAV2");
            if (r == 0)
            {
                String s = new String(facilityId_005_04).Trim();
                ofileNAV2.Write(s);
                ofileNAV2.Write('~');

                s = new String(name_043_30).Trim();
                ofileNAV2.Write(s);
                ofileNAV2.Write('~');

                remarks_029_600 = record.ToCharArray(28, 600);
                StringBuilder result = new StringBuilder();
                for (Int32 i = 0; i < remarks_029_600.Length; i++)
                {
                    Char c = remarks_029_600[i];
                    byte b = (byte)c;
                    if ((b < 32) || (b > 126))
                    {
                        // omitting non-ascii characters
                        // result.Append(replaceWith);
                    }
                    else
                    {
                        result.Append(c);
                    }
                }

                s = result.ToString().Trim();
                ofileNAV2.Write(s);
                ofileNAV2.Write(ofileNAV2.NewLine);
            }
        }
    }
}
