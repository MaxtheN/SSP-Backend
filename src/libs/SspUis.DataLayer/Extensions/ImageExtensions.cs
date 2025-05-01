using SspUis.Core.Security;
using StatusGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using WEBASE.DependencyInjection;
using WEBASE.Storage;

namespace SspUis.DataLayer;

public static class ImageExtensions
{
    public static IStatusGeneric AddFromTempFile(this ICollection<Guid> fileIds, string document, List<Guid> tempFileIds, Action<Guid> onAddEntity = null)
    {
        StatusGenericHandler statusGenericHandler = new StatusGenericHandler();
        IStorageService storageService = BaseServiceProvider<IAuthService>.StorageService;
        IStorageFileInfo[] tempFileInfos = storageService.GetTempFileInfos(document, tempFileIds.ToArray());
        if (storageService.IsValid && tempFileInfos.Length != tempFileIds.Count)
        {
            statusGenericHandler.AddError("Некоторые файлы найдены");
        }

        statusGenericHandler.CombineStatuses(storageService);
        if (statusGenericHandler.IsValid)
        {
            IStorageFileInfo[] array = tempFileInfos;
            foreach (IStorageFileInfo storageFileInfo in array)
            {
                Guid val = storageFileInfo.FileId;
                onAddEntity?.Invoke(val);
                fileIds.Add(val);
            }
        }

        return statusGenericHandler;
    }

    public static IStatusGeneric UpdateFromFile(this ICollection<Guid> fileIds, string document, string documentId, List<Guid> files, Action<Guid> onAddEntity = null)
    {
        StatusGenericHandler statusGenericHandler = new StatusGenericHandler();
        IStorageService storageService = BaseServiceProvider<IAuthService>.StorageService;
        List<Guid> list = files.ToList();
        List<Guid> list2 = new List<Guid>();
        foreach (Guid item in fileIds)
        {
            if (list.Contains(item))
            {
                list.Remove(item);
            }
            else
            {
                list2.Add(item);
            }
        }

        foreach (Guid item2 in list2)
        {
            fileIds.Remove(item2);
            storageService.MarkFileForDelete(document, documentId, item2);
        }

        statusGenericHandler.CombineStatuses(fileIds.AddFromTempFile(document, list, onAddEntity));
        if (statusGenericHandler.IsValid)
        {
            storageService.MarkFileForMoveToPersistent(document, documentId, list.ToArray());
        }

        return statusGenericHandler;
    }
}
