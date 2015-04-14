﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Ninject;
using NUnit.Framework;
using platformAthletic.Global.Auth;
using platformAthletic.Global.Config;
using platformAthletic.Mappers;
using platformAthletic.Model;

using System.Data.Linq;
using platformAthletic.UnitTest;

namespace platformAthletic.IntegrationTest
{
    [SetUpFixture]
    public class IntegrationTestSetupFixture : UnitTestSetupFixture
    {

        public class FileListRestore
        {
            public string LogicalName { get; set; }
            public string Type { get; set; }
        }

        protected static string NameDb = "platform2015_current";

        protected static bool RemoveAfter = false;

        protected static string TestDbName;

        protected override void InitRepository(StandardKernel kernel)
        {
           // FileInfo sandboxFile;
            string connectionString;
            //CopyDb(kernel, out sandboxFile, out connectionString);
            connectionString = "Data Source=ms-sql-8.in-solve.ru;Initial Catalog=1gb_platformath;User Id=1gb_chernikov;Password=994d312c;";
            kernel.Bind<platformAthleticDbDataContext>().ToMethod(c =>  new platformAthleticDbDataContext(connectionString));
            kernel.Bind<IRepository>().To<SqlRepository>();
            //sandboxFile.Delete();
        }

        private void CopyDb(StandardKernel kernel, out FileInfo sandboxFile, out string connectionString)
        {
            var config = kernel.Get<IConfig>();
            var db = new DataContext(config.ConnectionStrings("ConnectionString"));

            TestDbName = string.Format("{0}_{1}", NameDb, DateTime.Now.ToString("yyyyMMdd_HHmmss"));
            sandboxFile = new FileInfo(string.Format("{0}\\{1}.bak", Sandbox, TestDbName));
            var sandboxDir = new DirectoryInfo(Sandbox);

            //backupFile
            var textBackUp = string.Format(@"-- Backup the database
            BACKUP DATABASE [{0}]
            TO DISK = '{1}'
            WITH COPY_ONLY",
            NameDb, sandboxFile.FullName);
            db.ExecuteCommand(textBackUp);

            var restoreFileList = string.Format("RESTORE FILELISTONLY FROM DISK = '{0}'", sandboxFile.FullName);
            var fileListRestores = db.ExecuteQuery<FileListRestore>(restoreFileList).ToList();
            var logicalDbName = fileListRestores.FirstOrDefault(p => p.Type == "D");
            var logicalLogDbName = fileListRestores.FirstOrDefault(p => p.Type == "L");

            var restoreDb = string.Format("RESTORE DATABASE [{0}] FROM DISK = '{1}' WITH FILE = 1, MOVE N'{2}' TO N'{4}\\{0}.mdf', MOVE N'{3}' TO N'{4}\\{0}.ldf', NOUNLOAD, STATS = 10", TestDbName, sandboxFile.FullName, logicalDbName.LogicalName, logicalLogDbName.LogicalName, sandboxDir.FullName);
            db.ExecuteCommand(restoreDb);

            connectionString = config.ConnectionStrings("ConnectionString").Replace(NameDb, TestDbName);
        }

        [TearDown]
        public override void TearDown()
        {
            if (RemoveAfter)
            {
                RemoveDb();
            }
            Console.WriteLine("===============");
            Console.WriteLine("=====BYE!======");
            Console.WriteLine("===============");
        }

        private void RemoveDb()
        {
          /*  var config =  DependencyResolver.Current.GetService<IConfig>();

            var db = new DataContext(config.ConnectionStrings("ConnectionString"));

            var textCloseConnectionTestDb = string.Format(@"ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", TestDbName);
            db.ExecuteCommand(textCloseConnectionTestDb);

            var textDropTestDb = string.Format(@"DROP DATABASE [{0}]", TestDbName);
            db.ExecuteCommand(textDropTestDb);*/
        }
    }
}
