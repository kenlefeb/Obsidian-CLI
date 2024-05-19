using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Microsoft.Extensions.Logging;
using Obsidian.Domain.Abstractions.Services;
using Obsidian.Domain.Settings;

namespace Obsidian.Persistence
{
    public class Vault
    {
        private readonly ILogger<Vault> _logger;
        private readonly IFileSystem _filesystem;
        private readonly DailyNotes _dailyNotes;

        public Vault(ILogger<Vault> logger, IFileSystem filesystem, VaultSettings settings, IEnvironmentVariables environment, ITemplater templater)
        {
            _logger = logger;
            _filesystem = filesystem;
            Templater = templater;
            Environment = environment;
            Settings = settings;
            _dailyNotes = new DailyNotes(this);    // TODO: Replace with a Notes repository that includes DailyNotes
        }

        public bool Exists => _filesystem.Directory.Exists(Settings.Render(Templater).Path);

        public string Path => Settings.Path;
        public VaultSettings Settings { get; }
        public IEnvironmentVariables Environment { get; }
        public ITemplater Templater { get; }

        // TODO: Replace Create method with DI container
        public static Vault Create(ILogger<Vault> logger, IFileSystem filesystem, VaultSettings settings, IEnvironmentVariables environment, ITemplater templater)
        {
            var vault= new Vault(logger, filesystem, settings, environment, templater);
            if (!vault.Exists)
            {
                filesystem.Directory.CreateDirectory(vault.Path);
            }

            return vault;
        }

        public Note AddDailyNote(DateOnly date, bool force)
        {
            return new DailyNote(this, date, force);
        }

        public void WriteTextFile(string path, string contents)
        {
            _filesystem.File.WriteAllText(path, contents);
        }

        public string ReadTextFile(string path)
        {
            return _filesystem.File.ReadAllText(path);
        }

        public bool DirectoryExists(string path)
        {
            return _filesystem.Directory.Exists(path);
        }

        public IDirectoryInfo CreateDirectory(string path)
        {
            return _filesystem.Directory.CreateDirectory(path);
        }

        public IDirectoryInfo GetDirectory(string path)
        {
            return _filesystem.DirectoryInfo.New(path);
        }

        public IFileInfo GetFile(string path)
        {
            return _filesystem.FileInfo.New(path);
        }

        public IEnumerable<IFileInfo> GetFiles(string path)
        {
            return Directory.EnumerateFiles(System.IO.Path.Combine(Path, path), "*.*", SearchOption.AllDirectories)
                .Select(file => _filesystem.FileInfo.New(file));
        }

        public void DeleteFile(string path)
        {
            var file = _filesystem.FileInfo.New(path);
            file.Delete();
        }
    }
}
