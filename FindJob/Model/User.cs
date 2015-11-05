using System;

namespace FindJob.Model
{
    public class User
    {
        private string _name;
        private string _pwd;
        private string _email;
        private string _GUID;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Pwd
        {
            get { return _pwd; }
            set { _pwd = value; }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }

        public string Guid
        {
            get { return _GUID; }
            set { _GUID = value; }
        }
    }
}