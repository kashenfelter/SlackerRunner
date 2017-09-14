﻿//
using System;
using System.IO;

namespace SlackerRunner
{
  public class SlackerService
  {
    private readonly Func<User, IDisposable> _impersonatorCreator;
    private readonly ProfileRunner _profileRunner = new ProfileRunner();
    private int _timeout = 0;

    public SlackerService() : this((x) => new Impersonator(x.Name, x.Domain, x.Password), ProfileRunner.DEFAULT_TIMEOUT) { }

    public SlackerService(int timeoutMilliSeconds) : this((x) => new Impersonator(x.Name, x.Domain, x.Password), timeoutMilliSeconds) { }

    public SlackerService(Func<User, IDisposable> impersonatorCreator, int timeoutMilliseconds)
    {
      _timeout = timeoutMilliseconds;
      _impersonatorCreator = impersonatorCreator;
    }


    public int TimeoutMilliseconds
    {
      set { _timeout = value; }
      get { return _timeout; }
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
      using (_impersonatorCreator(user))
      {
        return _profileRunner.Run(testdirectory, testfile);
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
      // Make sure directory and file exists before heading further
      if (!Directory.Exists(testdirectory))
        throw new SlackerException("The directory does not exist, directory=" + testdirectory);

      // Only test for specific test file when wildcars are not in use
      if (testfile.IndexOf('*') == -1 && !File.Exists(testfile))
        throw new SlackerException("The file does not exist, file=" + testfile);

      // Go for it
      return _profileRunner.Run(testdirectory, testfile);
    }
    
    /// <summary>
    /// Runs the tests in the given directory and sub directories.
    /// </summary>
    /// <param name="testdirectory">Base directory where database.yml is located. </param>
    /// <param name="testfile">The test directory.</param>
    public SlackerResults RunDirectory(string testDirectory, string specDirectory, int timeoutMilliseconds)
    {
      // Make sure directory and file exists before heading further
      if (!Directory.Exists(testDirectory))
        throw new SlackerException("The directory does not exist, directory=" + testDirectory);

      // Go for it
      return _profileRunner.RunDirectory(testDirectory, specDirectory, timeoutMilliseconds);
    }

  }
}