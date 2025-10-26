# PDFUABox
~~TODO: License https://github.com/TalAloni/SMBLibrary klären~~ zu teuer

UML Diagramm
https://app.diagrams.net//?src=about#G1mXKXHjv7k9nY3b8v4JH1

Getting the connection string from a database
https://stackoverflow.com/questions/10479763/how-to-get-the-connection-string-from-a-database


    public JsonResult OnGetRowStatus(int rowId)
    {
        // Simulate fetching the status of the row from a database or service
        var status = GetRowStatusFromDatabase(rowId); // Replace with your logic
        return new JsonResult(new { RowId = rowId, Status = status });
    }

    private string GetRowStatusFromDatabase(int rowId)
    {
        // Example logic: Replace with actual database/service call
        return rowId % 2 == 0 ? "Completed" : "Pending";
    }
}

