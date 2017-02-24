using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CompareFolders
{
    public class CompareDirs
    {
        private string _OldVcp;
        private string _NewVcp;

        List<string> _lstSameResult = new List<string>();
        List<string> _lstDiffResult1 = new List<string>();
        List<string> _lstDiffResult2 = new List<string>();
        public List<string> lstSameResult1 { get { return _lstSameResult; }  }
        public List<string> lstDiffResult1 { get { return _lstDiffResult1; } }
        public List<string> lstDiffResult2 { get { return _lstDiffResult2; } }

        public CompareDirs(string OldVcp, string NewVcp)
        {
            _OldVcp = OldVcp;
            _NewVcp = NewVcp;
        }
        public void Compare(List<string> lstFileType)
        {

            // Create two identical or different temporary folders   
            // on a local drive and change these file paths.  


            System.IO.DirectoryInfo dir1 = new System.IO.DirectoryInfo(_OldVcp);
            System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(_NewVcp);

            // Take a snapshot of the file system.  
            IEnumerable<System.IO.FileInfo> list1 = dir1.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
            IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

            //A custom file comparer defined below  
            FileCompare myFileCompare = new FileCompare();

            // This query determines whether the two folders contain  
            // identical file lists, based on the custom file comparer  
            // that is defined in the FileCompare class.  
            // The query executes immediately because it returns a bool.  
            bool areIdentical = list1.SequenceEqual(list2, myFileCompare);

            // Find the common files. It produces a sequence and doesn't   
            // execute until the foreach statement.  
            var queryCommonFiles = list1.Intersect(list2, myFileCompare);
            if (queryCommonFiles.Count() > 0)
            {
                foreach (var v in queryCommonFiles)
                {
                    if (lstFileType.Count > 0)
                    {
                        if (lstFileType.Contains(v.Extension.ToUpper()))
                        {
                            _lstSameResult.Add(v.FullName);
                        }
                    }
                    else
                    {
                        _lstSameResult.Add(v.FullName);
                    }
                }
            }

            // Find the set difference between the two folders.  
            // For this example we only check one way.  
            var queryList1Only = (from file in list1
                                  select file ).Except(list2, myFileCompare);
            foreach (var v in queryList1Only)
            {
                if (lstFileType.Count > 0)
                {
                    if (lstFileType.Contains(v.Extension.ToUpper()))
                    {
                        _lstDiffResult1.Add(v.FullName);
                    }
                }
                else
                {
                    _lstDiffResult1.Add(v.FullName);
                }
            }

            // Find the set difference between the two folders.  
            // For this example we only check one way.  
            var queryList1Only1 = (from file in list2
                                  select file).Except(list1, myFileCompare);
            foreach (var v in queryList1Only1)
            {
                if (lstFileType.Count > 0)
                {
                    if (lstFileType.Contains(v.Extension.ToUpper()))
                    {
                        _lstDiffResult2.Add(v.FullName);
                    }
                }
                else
                {
                    _lstDiffResult2.Add(v.FullName);
                }
            }
        }
    }

    // This implementation defines a very simple comparison  
    // between two FileInfo objects. It only compares the name  
    // of the files being compared and their length in bytes.  
    class FileCompare : System.Collections.Generic.IEqualityComparer<System.IO.FileInfo>
    {
        public FileCompare() { }

        public bool Equals(System.IO.FileInfo f1, System.IO.FileInfo f2)
        {
            return (f1.Name == f2.Name &&
                    f1.Length == f2.Length);
        }

        // Return a hash that reflects the comparison criteria. According to the   
        // rules for IEqualityComparer<T>, if Equals is true, then the hash codes must  
        // also be equal. Because equality as defined here is a simple value equality, not  
        // reference identity, it is possible that two or more objects will produce the same  
        // hash code.  
        public int GetHashCode(System.IO.FileInfo fi)
        {
            string s = String.Format("{0}{1}", fi.Name, fi.Length);
            return s.GetHashCode();
        }
    }
}
