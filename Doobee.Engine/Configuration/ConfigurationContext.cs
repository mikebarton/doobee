using Doobee.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doobee.Engine.Configuration
{
    internal class ConfigurationContext
    {
        private IDataStorageProvider _storageProviderType;
        private int? _pageSizeInKb;
        private string _baseFolder;
        private int? _pageIncreaseNum;
        private int? _defaultBranchingFactor;


        public IDataStorageProvider StorageProviderType { get => _storageProviderType; set => _storageProviderType = value; }

        internal void Validate()
        {
            if (!_pageIncreaseNum.HasValue) _pageIncreaseNum = 4;
            if (!_pageSizeInKb.HasValue) _pageSizeInKb = 4096;
            if (!_defaultBranchingFactor.HasValue) _defaultBranchingFactor = 10;

            if (_storageProviderType == null)
                throw new InvalidProgramException("an instance of IDataStorageProvider must be specified when creating the database");

            if (string.IsNullOrEmpty(_baseFolder) && !(_storageProviderType is MemoryStorageProvider))
                throw new InvalidProgramException("a file location must be specified when creating the database unless using a MemoryStorageProvider");

            if (!string.IsNullOrEmpty(_baseFolder) && _storageProviderType is MemoryStorageProvider)
                throw new InvalidProgramException("a file location must not be specified when creating the database with a MemoryStorageProvider");


        }

        public int? PageSizeInKb { get => _pageSizeInKb; set => _pageSizeInKb = value; }
        public string BaseFolder { get => _baseFolder; set => _baseFolder = value; }
        public int? PageIncreaseNum { get => _pageIncreaseNum; set => _pageIncreaseNum = value; }
        public int? DefaultBranchingFactor { get => _defaultBranchingFactor; set => _defaultBranchingFactor = value; }
    }
}
