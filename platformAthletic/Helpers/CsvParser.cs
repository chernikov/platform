using platformAthletic.Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace platformAthletic.Helpers
{
    class CsvParser : IDisposable
    {
        public Dictionary<string, byte> Position = new Dictionary<string, byte> {
            {"FirstName", 0},
            {"LastName", 1},
            {"Email", 2}
        };
        /// <summary>
        /// path to file
        /// </summary>
        protected string Path;

        /// <summary>
        /// Instance stream file which is parsed
        /// </summary>
        private StreamReader FileStreamReader;

        /// <summary>
        /// column separator in csv file
        /// </summary>
        private char ColumnSeparator = ';';

        /// <summary>
        /// Position in stream where begin data about players, and where headers end
        /// </summary>
        private long StartDataPos = 0;

        // Track whether Dispose has been called.
        private bool disposed = false;

        private List<string> LinesList;

        private bool IsHeadersValid = false;




        public CsvParser(string path)
        {
            this.Path = path;
            this.FileStreamReader = new StreamReader(path, Encoding.Default);
            this.StreamToList();
            this.DefineColumnSeparator();
            this.CheckHeaders();


        }

        public CsvParser(Stream stream)
        {
            this.FileStreamReader = new StreamReader(stream, Encoding.Default);
            this.StreamToList();
            this.DefineColumnSeparator();
            this.CheckHeaders();
        }


        /// <summary>
        /// Parse csv file to json and return him
        /// </summary>
        public string ParseToJson()
        {
            List<UserInfo> toJSON = new List<UserInfo>();

            //string line;
            string[] elemets;
            List<string> lines = this.IsHeadersValid ? this.LinesList.Skip(1).ToList() : this.LinesList; ;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //this.FileStreamReader.BaseStream.Seek(this.StartDataPos, SeekOrigin.Begin);
            //this.FileStreamReader.DiscardBufferedData();
            //while ((line = this.FileStreamReader.ReadLine()) != null)
            foreach (string line in lines)
            {
                elemets = line.Split(this.ColumnSeparator);
                byte notEmptyElemetCount = (byte)elemets.Where(e => !String.IsNullOrEmpty(e) && !String.IsNullOrWhiteSpace(e)).ToList().Count;
                if (elemets.Length <= 3 && notEmptyElemetCount > 0 && notEmptyElemetCount <= 3)
                {
                    toJSON.Add(new UserInfo()
                    {
                        FirstName = this.Position["FirstName"] < notEmptyElemetCount ?
                                    EscapeData(elemets[this.Position["FirstName"]]) : String.Empty,
                        LastName  = this.Position["LastName"] < notEmptyElemetCount ?
                                    EscapeData(elemets[this.Position["LastName"]]) : String.Empty,
                        Email     = this.Position["Email"] < notEmptyElemetCount ?
                                    EscapeData(elemets[this.Position["Email"]]) : String.Empty
                    });
                }
            }
            var result = serializer.Serialize(toJSON);
            return result;

        }//end method


        /// <summary>
        /// Parse csv and return BatchPlayersView instance
        /// </summary>
        /// <returns>BatchPlayersView</returns>
        public BatchPlayersView Parse()
        {
            //string line;
            string[] elemets;
            List<string> lines = this.IsHeadersValid ? this.LinesList.Skip(1).ToList() : this.LinesList; ;
            BatchPlayersView batchPlayersView = new BatchPlayersView();
            //this.FileStreamReader.BaseStream.Seek(this.StartDataPos, SeekOrigin.Begin);
            //this.FileStreamReader.DiscardBufferedData();
            //while ((line = this.FileStreamReader.ReadLine()) != null)
            foreach (string line in lines)
            {
                elemets = line.Split(this.ColumnSeparator);
                byte notEmptyElemetCount = (byte)elemets.Where(e => !String.IsNullOrEmpty(e) && !String.IsNullOrWhiteSpace(e)).ToList().Count;
                if (elemets.Length <= 3 && notEmptyElemetCount > 0 && notEmptyElemetCount <= 3)
                {
                    batchPlayersView.Players.Add(Guid.NewGuid().ToString("N"), new PlayerView()
                    {
                        FirstName = this.Position["FirstName"] < notEmptyElemetCount ?
                                    EscapeData(elemets[this.Position["FirstName"]]) : String.Empty, 
                        LastName  = this.Position["LastName"] < notEmptyElemetCount ?
                                    EscapeData(elemets[this.Position["LastName"]]) : String.Empty,
                        Email     = this.Position["Email"] < notEmptyElemetCount ?
                                    EscapeData(elemets[this.Position["Email"]]) : String.Empty
                    });
                }
            }
            return batchPlayersView;
        }

        private void DefineColumnSeparator()
        {
            const char comma = ',';
            const char semicolon = ';';
            //uint cursorPos = (uint)this.FileStreamReader.BaseStream.Position;
            //this.FileStreamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            //this.FileStreamReader.DiscardBufferedData();
            //string line = this.FileStreamReader.ReadLine();
            string line = this.LinesList[0];
            string patternForComma = @"^(.*[^,]),(.*[^,]),(.*[^,])$";
            string patternForSemicolon = @"^(.*[^;]);(.*[^;]);(.*[^;])$";
            if (Regex.IsMatch(line, patternForSemicolon, RegexOptions.Compiled | RegexOptions.IgnoreCase))
            {
                this.ColumnSeparator = semicolon;
            }
            else if (Regex.IsMatch(line, patternForComma, RegexOptions.Compiled | RegexOptions.IgnoreCase))
            {
                this.ColumnSeparator = comma;
            }

            //this.FileStreamReader.BaseStream.Seek(cursorPos, SeekOrigin.Begin);
            //this.FileStreamReader.DiscardBufferedData();

        }

        private void StreamToList()
        {
            string line;
            List<string> list = new List<string>();
            while ((line = this.FileStreamReader.ReadLine()) != null)
            {
                list.Add(line);
            }
            this.LinesList = list;
        }

        /// <summary>
        /// Check Headers position
        /// </summary>
        public bool CheckHeaders()
        {
            bool checkedFirstName = false;
            bool checkedLastName = false;
            bool checkedEmail = false;

            string patternFirstName = @"^first\s+name$";
            string patternLastName = @"^last\s+name$";
            string patternEmail = @"^email$";

            Dictionary<string, byte> tempPosition = new Dictionary<string, byte>(this.Position);
            //uint cursorPos = (uint)this.FileStreamReader.BaseStream.Position;
            //this.FileStreamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            //this.FileStreamReader.DiscardBufferedData();
            //string[] line = this.FileStreamReader.ReadLine().Split(this.ColumnSeparator);
            string[] line = this.LinesList[0].Split(this.ColumnSeparator);

            for (byte i = 0; i < line.Length; i++)
            {
                string key;

                if (Regex.IsMatch(line[i].Trim(' '), patternFirstName, RegexOptions.IgnoreCase) && !checkedFirstName)
                {
                    if (tempPosition["FirstName"] != i)
                    {
                        key = tempPosition.FirstOrDefault(x => x.Value == i).Key;
                        tempPosition[key] = Position["FirstName"];
                    }
                    tempPosition["FirstName"] = i;
                    checkedFirstName = true;
                }

                else if (Regex.IsMatch(line[i].Trim(' '), patternLastName, RegexOptions.IgnoreCase) && !checkedLastName)
                {
                    if (tempPosition["LastName"] != i)
                    {
                        key = tempPosition.FirstOrDefault(x => x.Value == i).Key;
                        tempPosition[key] = Position["LastName"];
                    }
                    tempPosition["LastName"] = i;
                    checkedLastName = true;
                }

                else if (Regex.IsMatch(line[i].Trim(' '), patternEmail, RegexOptions.IgnoreCase) && !checkedEmail)
                {
                    if (tempPosition["Email"] != i)
                    {
                        key = tempPosition.FirstOrDefault(x => x.Value == i).Key;
                        tempPosition[key] = Position["Email"];
                    }
                    tempPosition["Email"] = i;
                    checkedEmail = true;
                }
            }

            if (checkedFirstName && checkedLastName && checkedEmail)
            {
                this.Position = new Dictionary<string, byte>(tempPosition);
                this.IsHeadersValid = true;
                //this.StartDataPos = this.FileStreamReader.BaseStream.Position;
            }
            else
            {
                this.IsHeadersValid = false;
                //this.FileStreamReader.BaseStream.Seek(cursorPos, SeekOrigin.Begin);
                //this.FileStreamReader.DiscardBufferedData();
            }

            return checkedFirstName && checkedLastName && checkedEmail;

        } // end method

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed
                // and unmanaged resources.
                if (disposing)
                {
                    // Dispose managed resources.
                }

                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // If disposing is false,
                // only the following code is executed.
                this.FileStreamReader.Dispose();
                // Note disposing has been done.
                disposed = true;

            }
        }

        // Use C# destructor syntax for finalization code.
        // This destructor will run only if the Dispose method
        // does not get called.
        // It gives your base class the opportunity to finalize.
        // Do not provide destructors in types derived from this class.
        ~CsvParser()
        {
            // Do not re-create Dispose clean-up code here.
            // Calling Dispose(false) is optimal in terms of
            // readability and maintainability.
            Dispose(false);
        }

        protected string EscapeData(string data)
        {
            return HttpUtility.HtmlEncode(data.Replace(">", "&#62;").Replace("<", "&#60;").Trim());
        }

    } /*end class*/

    class UserInfo
    {
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = EscapeData(value);
                FirstNameValid = CheckFirstNameValid();
            }
        }


        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = EscapeData(value);
                LastNameValid = CheckLastNameValid();
            }
        }


        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = EscapeData(value);
                EmailValid = CheckEmailValid();
            }
        }

        public bool FirstNameValid { get; set; }
        public bool LastNameValid { get; set; }
        public bool EmailValid { get; set; }


        protected string _FirstName;
        protected string _LastName;
        protected string _Email;


        public bool CheckFirstNameValid()
        {
            return !(String.IsNullOrWhiteSpace(FirstName) || String.IsNullOrEmpty(FirstName));

        }


        public bool CheckLastNameValid()
        {
            return !(String.IsNullOrWhiteSpace(LastName) || String.IsNullOrEmpty(LastName));
        }


        public bool CheckEmailValid()
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

            return !(String.IsNullOrWhiteSpace(Email) || String.IsNullOrEmpty(Email)) ||
                Regex.IsMatch(Email, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        protected string EscapeData(string data)
        {
            return HttpUtility.HtmlEncode(data.Replace(">", "&#62;").Replace("<", "&#60;"));
        }
    }
}