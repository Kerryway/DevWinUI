﻿using System.Text.Json.Serialization;

namespace DevWinUI;

public partial class ContextMenuItem : ContextMenuBaseModel
{
    [JsonIgnore]
    public StorageFile File { get; set; }

    [JsonIgnore]
    public string FileName
    {
        get => File?.Name;
        set
        {
            OnPropertyChanged(nameof(FileName));
        }
    }

    private bool _enabled;
    [JsonIgnore]
    public bool Enabled
    {
        get => _enabled;
        set => SetProperty(ref _enabled, value);
    }

    private string _title;
    private int _index;
    private string _exe;
    private string _param;
    private string _icon;
    private string _iconDark;

    private bool _acceptDirectory;
    private int _acceptDirectoryFlag;

    private bool _acceptFile;
    private int _acceptFileFlag;
    private string _acceptExts;
    private string _acceptFileRegex;

    private int _acceptMultipleFilesFlag;
    private string _pathDelimiter;
    private string _paramForMultipleFiles;

    private int _showWindowFlag;
    private string _workingDirectory;

    [JsonPropertyOrder(0)]
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    [JsonPropertyOrder(2)]
    public int Index
    {
        get => _index;
        set => SetProperty(ref _index, value);
    }

    [JsonPropertyOrder(4)]
    public string Exe
    {
        get => _exe;
        set => SetProperty(ref _exe, value);
    }

    [JsonPropertyOrder(6)]
    public string Param
    {
        get => _param;
        set => SetProperty(ref _param, value);
    }

    [JsonPropertyOrder(8)]
    public string Icon
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }

    [JsonPropertyOrder(10)]
    public string IconDark
    {
        get => _iconDark;
        set => SetProperty(ref _iconDark, value);
    }

    [Obsolete()]
    [JsonPropertyOrder(20)]
    public bool AcceptDirectory
    {
        get => _acceptDirectory;
        set => SetProperty(ref _acceptDirectory, value);
    }
    public bool ShouldSerializeAcceptDirectory() => false;

    [JsonPropertyOrder(22)]
    public int AcceptDirectoryFlag
    {
        get => _acceptDirectoryFlag;
        set => SetProperty(ref _acceptDirectoryFlag, value);
    }

    [Obsolete()]
    [JsonPropertyOrder(30)]
    public bool AcceptFile
    {
        get => _acceptFile;
        set => SetProperty(ref _acceptFile, value);
    }
    public bool ShouldSerializeAcceptFile() => false;

    [JsonPropertyOrder(32)]
    public int AcceptFileFlag
    {
        get => _acceptFileFlag;
        set => SetProperty(ref _acceptFileFlag, value);
    }

    [JsonPropertyOrder(34)]
    public string AcceptExts
    {
        get => _acceptExts;
        set => SetProperty(ref _acceptExts, string.IsNullOrEmpty(value) ? value : value.ToLower());
    }

    [JsonPropertyOrder(36)]
    public string AcceptFileRegex
    {
        get => _acceptFileRegex;
        set => SetProperty(ref _acceptFileRegex, value);
    }

    [JsonPropertyOrder(40)]
    public int AcceptMultipleFilesFlag
    {
        get => _acceptMultipleFilesFlag;
        set => SetProperty(ref _acceptMultipleFilesFlag, value);
    }

    [JsonPropertyOrder(42)]
    public string PathDelimiter
    {
        get => _pathDelimiter;
        set => SetProperty(ref _pathDelimiter, value);
    }

    [JsonPropertyOrder(44)]
    public string ParamForMultipleFiles
    {
        get => _paramForMultipleFiles;
        set => SetProperty(ref _paramForMultipleFiles, value);
    }

    [JsonPropertyOrder(50)]
    public int ShowWindowFlag
    {
        get => _showWindowFlag;
        set => SetProperty(ref _showWindowFlag, value);
    }

    [JsonPropertyOrder(52)]
    public string WorkingDirectory
    {
        get => _workingDirectory;
        set => SetProperty(ref _workingDirectory, value);
    }
}
