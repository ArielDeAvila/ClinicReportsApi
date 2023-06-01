using ClinicReportsAPI.Data.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace ClinicReportsAPI.Tools;

public static class TemplateReport
{
    public static byte[] GenerateDocument(Report report)
    {
        var document = Document.Create(document =>
        {

            document.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.MarginVertical(1.5f, Unit.Centimetre);
                page.MarginHorizontal(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(s => s.FontSize(10));

                page.Header().Row(row =>
                {
                    row.RelativeItem().AlignLeft().AlignMiddle().Text("System Report").Bold().FontSize(20);

                    row.RelativeItem().Column(col =>
                    {
                        col.Item().AlignCenter().Text(report.Hospital.Name).FontSize(14).Bold();
                        col.Item().AlignCenter().Text(report.Hospital.Address).FontSize(10);
                        col.Item().AlignCenter().Text($"Teléfono: {report.Hospital.PhoneNumber}").FontSize(10);
                        col.Item().AlignCenter().Text($"NIT: {report.Hospital.Identification}").FontSize(10);

                    });

                    row.RelativeItem().AlignMiddle().AlignRight().Table(table =>
                    {
                        //definir el número de columnas
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(80);
                        });

                        table.Cell().Text("Generado").FontSize(9);
                        table.Cell().Text($"{report.GenerateDate}").FontSize(8);
                        table.Cell().Text("").FontSize(5);
                        table.Cell().Text("").FontSize(5);
                        table.Cell().Text("Nro reporte").FontSize(9);
                        table.Cell().Text($"{report.Id}").FontSize(8);


                    });

                });

                page.Content().PaddingVertical(10).Column(content =>
                {
                    content.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(80);
                            columns.RelativeColumn();
                        });

                        table.Cell().Text("Servicio").Bold();
                        table.Cell().Text("CONSULTA MEDICO ESPACIALISTA (ARTRITIS REUMATOIDE)");
                        table.Cell().Text("Médico").Bold();
                        table.Cell().Text(report.Doctor.Name);
                        table.Cell().Text("Especialidad").Bold();
                        table.Cell().Text(report.Doctor.MedicalSpecialty);

                    });

                    content.Item().PaddingVertical(10).Table(table =>
                    {
                        table.ExtendLastCellsToTableBottom();

                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(110);
                            columns.RelativeColumn();
                            columns.ConstantColumn(110);
                            columns.RelativeColumn();
                        });

                        table.Cell().ColumnSpan(4).Background("#D9D9D9").AlignCenter().Text("IMFORMACIÓN PACIENTE").Bold();

                        table.Cell().PaddingTop(5).Text("Documento").SemiBold();
                        table.Cell().PaddingTop(5).Text(report.Patient.Identification);

                        table.Cell().PaddingTop(5).Text("Fecha de nacimiento").SemiBold();
                        table.Cell().PaddingTop(5).Text(report.Patient.BirthDate.ToShortDateString());

                        table.Cell().Text("Nombre").SemiBold();
                        table.Cell().Text(report.Patient.Name);

                        table.Cell().Text("Teléfono").SemiBold();
                        table.Cell().Text(report.Patient.PhoneNumber);

                        table.Cell().Text("Dirección").SemiBold();
                        table.Cell().Text(report.Patient.Address);

                        table.Cell().Text("Sexo").SemiBold();
                        table.Cell().Text($"{report.Patient.Gender}");

                        table.Cell().PaddingBottom(5).Text("Edad").SemiBold();
                        table.Cell().PaddingBottom(5).Text(Edad(report));

                        table.Cell().ColumnSpan(4).Background("#D9D9D9").AlignCenter().Text("DIAGNOSTICOS").Bold();

                        table.Cell().PaddingVertical(5).Text("Diagnóstico ppal").SemiBold();
                        table.Cell().ColumnSpan(3).PaddingVertical(5).Text(report.Diagnosis);

                        table.Cell().ColumnSpan(4).Background("#D9D9D9").AlignCenter().Text("OBSERVACIONES").Bold();
                        table.Cell().ColumnSpan(4).PaddingVertical(5).Text(report.Observation);
                        
                        table.Cell().ColumnSpan(4).Background("#D9D9D9").AlignCenter().Text("TRATAMIENTO").Bold();
                        table.Cell().ColumnSpan(4).PaddingVertical(5).Text(report.Treatment);





                    });
                });

                page.Footer().AlignRight().Text(txt =>
                {
                    txt.Span("Página: ").FontSize(10);
                    txt.CurrentPageNumber().FontSize(10);
                    txt.Span(" de ").FontSize(10);
                    txt.TotalPages().FontSize(10);

                });

            });



        }).GeneratePdf();

        return document;
    }

    private static string Edad(Report report)
    {
        DateTime reportDate = report.GenerateDate;
        DateTime BirthDate = report.Patient.BirthDate;

        int años = reportDate.Year - BirthDate.Year;
        int meses = reportDate.Month - BirthDate.Month;

        // Verificar si todavía no se ha cumplido el día de nacimiento en el mes actual
        if (BirthDate > reportDate.AddYears(-años).AddMonths(-meses))
        {
            meses--;
        }

        // Ajustar meses si el resultado es negativo
        if (meses < 0)
        {
            meses = 12 + meses;
            años--;
        }

        return años + " años " + meses + " meses";
    }
}
