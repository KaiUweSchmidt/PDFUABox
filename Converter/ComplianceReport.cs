// Pseudocode (Plan):
// 1. Erstelle Klasse `Compliance` als Root mit Attributen: Name, Operation, Target.
// 2. Compliance enthält einfache Elemente: Version, Copyright, Date.
// 3. Compliance enthält ein `File`-Element -> Klasse `FileItem` mit Attributen Version, Name, Pages.
// 4. `FileItem` enthält viele Knoten (Security, General, Text, Fonts, Graphics, Headings, Tables,
//    Lists, NotesAndReferences, OptionalContent, EmbeddedFiles, DigitalSignatures,
//    NonInteractiveForms, XFA, Navigation, Annotations, Actions, XObjects, VersionIdentification).
// 5. Viele dieser Knoten enthalten wiederholbare `Problem`-Elemente.
//    - Manche Dateien verwenden den Elementnamen "Problem", andere "Problems".
//    - Implementiere `Category` so, dass es beide möglichen XML-Namen akzeptiert:
//      [XmlElement("Problem"), XmlElement("Problems")] List<Problem> Problems
// 6. Definiere Klasse `Problem` mit Attributen: Severity, Clause, ObjectID, Page, Convertable, Code
//    und dem inneren Text als Beschreibung ([XmlText]).
// 7. Für `Annotations` verwende eine eigene Klasse `Annotations` oder wieder die `Category`.
// 8. Markiere Klassen mit `XmlRoot` / `XmlType` / `XmlElement` / `XmlAttribute` für XmlSerializer.
// 9. Verwende strings für Felder wie Page/ObjectID/Pages, um leere oder nicht-numerische Werte zu erlauben.
// 10. Generiere die C#-Datei mit Namespace `Models` und nötigen using-Anweisungen.
// Ende Pseudocode

using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Converter.ComplianceReportModels
{
    [XmlRoot("Compliance", IsNullable = true)]
    public class Compliance
    {
        [XmlAttribute("Name")]
        public string? Name { get; set; }

        [XmlAttribute("Operation")]
        public string? Operation { get; set; }

        [XmlAttribute("Target")]
        public string? Target { get; set; }

        [XmlElement("Version")]
        public string? Version { get; set; }

        [XmlElement("Copyright")]
        public string? Copyright { get; set; }

        [XmlElement("Date")]
        public string? Date { get; set; }

        [XmlElement("File")]
        public FileItem? File { get; set; }
    }

    public class FileItem
    {
        [XmlAttribute("Version")]
        public string? Version { get; set; }

        [XmlAttribute("Name")]
        public string? Name { get; set; }

        [XmlAttribute("Pages")]
        public string? Pages { get; set; }

        [XmlElement("Security")]
        public Category? Security { get; set; }

        [XmlElement("General")]
        public Category? General { get; set; }

        [XmlElement("Text")]
        public Category? Text { get; set; }

        [XmlElement("Fonts")]
        public Category? Fonts { get; set; }

        [XmlElement("Graphics")]
        public Category? Graphics { get; set; }

        [XmlElement("Headings")]
        public Category? Headings { get; set; }

        [XmlElement("Tables")]
        public Category? Tables { get; set; }

        [XmlElement("Lists")]
        public Category? Lists { get; set; }

        [XmlElement("NotesAndReferences")]
        public Category? NotesAndReferences { get; set; }

        [XmlElement("OptionalContent")]
        public Category? OptionalContent { get; set; }

        [XmlElement("EmbeddedFiles")]
        public Category? EmbeddedFiles { get; set; }

        [XmlElement("DigitalSignatures")]
        public Category? DigitalSignatures { get; set; }

        [XmlElement("NonInteractiveForms")]
        public Category? NonInteractiveForms { get; set; }

        [XmlElement("XFA")]
        public Category? XFA { get; set; }

        [XmlElement("Navigation")]
        public Category? Navigation { get; set; }

        [XmlElement("Annotations")]
        public Category? Annotations { get; set; }

        [XmlElement("Actions")]
        public Category? Actions { get; set; }

        [XmlElement("XObjects")]
        public Category? XObjects { get; set; }

        [XmlElement("VersionIdentification")]
        public string? VersionIdentification { get; set; }
    }

    // Category accepts both <Problem .../> and <Problems .../> elements as items
    public class Category
    {
        [XmlElement("Problem", typeof(Problem))]
        [XmlElement("Problems", typeof(Problem))]
        public Collection<Problem>? Problems { get; } = new Collection<Problem>();
    }

    public class Problem
    {
        [XmlAttribute("Severity")]
        public string? Severity { get; set; }

        [XmlAttribute("Clause")]
        public string? Clause { get; set; }

        [XmlAttribute("ObjectID")]
        public string? ObjectID { get; set; }

        [XmlAttribute("Page")]
        public string? Page { get; set; }

        [XmlAttribute("Convertable")]
        public string? Convertable { get; set; }

        [XmlAttribute("Code")]
        public string? Code { get; set; }

        [XmlText]
        public string? Description { get; set; }
    }
}