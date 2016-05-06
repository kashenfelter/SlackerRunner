﻿//
using System;
using System.IO;

namespace SlackerRunner
{
    public class SlackerService
    {
        private readonly Func<User, IDisposable> impersonatorCreator;
        private readonly ProfileRunner profileRunner;

        public SlackerService() : this((x) => new Impersonator(x.Name, x.Domain, x.Password), new ProfileRunner()) { }
        
        public SlackerService(Func<User, IDisposable> impersonatorCreator, ProfileRunner profileRunner)
        {
            this.impersonatorCreator = impersonatorCreator;
            this.profileRunner = profileRunner;
        }

        /// <summary>
        /// Runs the test in the given testfile.
        /// </summary>
        /// <param name="testdirectory">Base directory where database.yml is located. </param>
        /// <param name="testfile">The test file to run.</param>
        /// <param name="user">Imprisonate user.</param>
        /// <returns></returns>
        public SlackerResults Run(string testdirectory, string testfile, User user)
        {
            using (impersonatorCreator(user))
            {
                return profileRunner.Run(testdirectory, testfile);
            }
        }

        /// <summary>
        /// Runs the test in the given testfile.
        /// </summary>
        /// <param name="testdirectory">Base directory where database.yml is located. </param>
        /// <param name="testfile">The test file to run.</param>
        /// <returns></returns>
        public SlackerResults Run(string testdirectory, string testfile)
        {
            // Make sure directory and file exist before heading further
            if (!Directory.Exists(testdirectory))
              throw new SlackerException("The directory does not exist, directory=" + testdirectory);      
            if (!File.Exists(testfile))
              throw new SlackerException("The file does not exist, file=" + testfile);

            // Go for it
            return profileRunner.Run(testdirectory, testfile);
        }


    }
}