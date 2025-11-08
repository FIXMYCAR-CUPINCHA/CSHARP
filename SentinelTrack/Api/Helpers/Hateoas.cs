using Microsoft.AspNetCore.Mvc;

namespace SentinelTrack.Api.Helpers;

public static class Hateoas
{
    public static object PageLinks(IUrlHelper url, string action, int page, int pageSize, int totalPages, object? extra = null)
    {
        var next = page < totalPages
            ? url.ActionLink(action, values: Merge(new { page = page + 1, pageSize }, extra))
            : null;

        var prev = page > 1
            ? url.ActionLink(action, values: Merge(new { page = page - 1, pageSize }, extra))
            : null;

        return new
        {
            self = url.ActionLink(action, values: Merge(new { page, pageSize }, extra)),
            next,
            prev
        };
    }

    public static object ItemLinks(IUrlHelper url, string getById, string list, string update, string delete, Guid id)
    {
        return new
        {
            self = url.ActionLink(getById, values: new { id }),
            list = url.ActionLink(list),
            update = url.ActionLink(update, values: new { id }),
            delete = url.ActionLink(delete, values: new { id })
        };
    }

    private static object Merge(object a, object? b)
    {
        if (b is null) return a;

        var dict = a.GetType().GetProperties()
            .ToDictionary(p => p.Name, p => p.GetValue(a));

        foreach (var prop in b.GetType().GetProperties())
            dict[prop.Name] = prop.GetValue(b);

        return dict;
    }
}