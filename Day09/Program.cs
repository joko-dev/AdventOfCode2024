using SharedKernel;
using System.Runtime.CompilerServices;

namespace Day09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 9: Disk Fragmenter"));
            Console.WriteLine("disk map: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<DiskElement> diskElements = getDiskElements(puzzleInput.Lines[0]);
            List<DiskElement> arrangedDiskElements = arrangeDiskElementsFragementation(diskElements);

            Console.WriteLine("Checksum fragmentation: {0}", getChecksum(arrangedDiskElements));

            arrangedDiskElements = arrangeDiskElementsWithoutFragementation(diskElements);
            Console.WriteLine("Checksum filled gaps: {0}", getChecksum(arrangedDiskElements));
        }

        private static List<DiskElement> arrangeDiskElementsWithoutFragementation(List<DiskElement> diskElements)
        {
            List<DiskElement> arranged = diskElements.ToList();

            for (int i = arranged.Count - 1; i >= 0; i--)
            {
                DiskElement fileElement = arranged[i];

                if (fileElement.IsFile)
                {
                    for (int j = 0; j < i; j++)
                    {
                        if (!arranged[j].IsFile && arranged[j].Length >= fileElement.Length)
                        {
                            DiskElement toRemove = arranged[j];

                            arranged.RemoveAt(i);
                            arranged.Insert(i, new DiskElement(fileElement.Length, 0, false));
                            arranged.RemoveAt(j);
                            arranged.Insert(j, fileElement);
                            

                            if (toRemove.Length > fileElement.Length)
                            {
                                arranged.Insert(j + 1, new DiskElement(toRemove.Length - fileElement.Length, 0, false));
                                i++;
                            }

                            break;
                        }
                    }
                }
            }

            return arranged;
        }

        private static List<DiskElement> arrangeDiskElementsFragementation(List<DiskElement> diskElements)
        {
            List<DiskElement> arranged = new List<DiskElement>();

            foreach (DiskElement diskElement in diskElements)
            {
                for (int i = 0; i < diskElement.Length; i++)
                {
                    arranged.Add(new DiskElement(1, diskElement.ID, diskElement.IsFile));
                }
            }

            int lastSwappedIndex = 0;
            for (int i = arranged.Count - 1; i >= 0; i--)
            {
                DiskElement fileElement = arranged[i];

                if (fileElement.IsFile)
                {
                    for (int j = lastSwappedIndex; j < i; j++)
                    {
                        if (!arranged[j].IsFile)
                        {
                            arranged[i] = arranged[j];
                            arranged[j] = fileElement;
                            lastSwappedIndex = j;
                            break;
                        }
                    }
                }
            }

            return arranged;
        }

        private static List<DiskElement> getDiskElements(string line)
        {
            List<DiskElement> diskElements = new List<DiskElement>();
            int fileId = 0;

            for (int i = 0; i < line.Length; i++)
            {
                if(i % 2 == 0)
                {
                    diskElements.Add(new DiskElement(Int32.Parse(line[i].ToString()), fileId, true));
                    fileId++;
                }
                else
                {
                    diskElements.Add(new DiskElement(Int32.Parse(line[i].ToString()), 0, false));
                }
            }

            return diskElements;
        }

        private static Int64 getChecksum(List<DiskElement> arrangedDiskElements)
        {
            Int64 checksum = 0;
            int blockId = 0;

            for (int i = 0; i < arrangedDiskElements.Count; i++)
            {
                DiskElement diskElement = arrangedDiskElements[i];

                for (int j = 1; j <= diskElement.Length; j++)
                {
                    if (diskElement.IsFile)
                    {
                        checksum += blockId * diskElement.ID;
                    }
                    blockId++;
                }
            }

            return checksum;
        }

        static void printArranged(List<DiskElement> arranged)
        {
            Console.Write("\n == \n");
            foreach (DiskElement diskElement in arranged)
            {
                for (int i = 0; i < diskElement.Length; i++) 
                {
                    if (diskElement.IsFile)
                    {
                        Console.Write(diskElement.ID);
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }

            }
        }
    }
}