using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using ResultOptionsClassLibrary;
using ResultTypesClassLibrary;
using System.IO;

using OpenXML_DocumentCreator;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;



namespace DataBase_Sheild_M
{
    public partial class SaverResultForm : Form
    {
        public SaverResultForm()
        {
            InitializeComponent();
        }
        
       public ResultType_MAINClass ResultForExport=null;

       #region сохранение в текстовый файл

       public static void SaveFrequencyElementToTextFile(string SaveFolderPath, FrequencyElementClass Result)
       {
          string SaveFilePath = string.Format("{0}\\{1}.txt", SaveFolderPath, Result.ToString());

          RichTextBox RTB = new RichTextBox();
           //RTB.AppendText(string.Format("Частота {0}\n", Result.ToString()));
           RTB.AppendText(string.Format("Координата, град \t Амплитуда, дБ \t Фаза, град \n\n"));

           foreach (ResultElementClass re in Result.ResultAmpl_PhaseElements)
           {
               RTB.AppendText(string.Format("{0} \t {1} \t {2} \n", re.Cordinate, re.Ampl_dB, re.Phase_degree));
           }

           RTB.SaveFile(SaveFilePath, RichTextBoxStreamType.PlainText);
       }

        public static void SavePolarizationElementToTextFile(string SaveFolderPath, PolarizationElementClass Result, bool InOneFile = false,string SpesialText="",string SpesialName="")
        {
            string ParalizStype = "";
            if (Result.Polarization != SelectedPolarizationEnum.None)
                ParalizStype = Result.Polarization.ToString();

            string SaveFilePath = string.Format("{0}\\{1} {2}", SaveFolderPath, SpesialName, ParalizStype);

            if (InOneFile)
            {
                RichTextBox RTB = new RichTextBox();
                RichTextBox RTB2 = new RichTextBox();
                //RTB.AppendText(string.Format("Частота {0}\n", Result.ToString()));


                for (int i = 0; i < Result.FrequencyElements[0].ResultAmpl_PhaseElements.Count; i++)
                {
                    string FreqLine = "";
                    string Line = "";
                    string Line2 = "";


                    for (int j = 0; j < Result.FrequencyElements.Count; j++)
                    {
                        if (i == 0)
                        {
                            if (FreqLine == "")
                                FreqLine = Math.Round(Result.FrequencyElements[j].Frequency, 2).ToString();
                            else
                                FreqLine = string.Format("{0}\t{1}", FreqLine, Math.Round(Result.FrequencyElements[j].Frequency, 2));
                        }

                        if (Line == "")
                        {
                            Line = Math.Round(Result.FrequencyElements[j].ResultAmpl_PhaseElements[i].Ampl_dB, 2).ToString();
                            Line2 = Math.Round(Result.FrequencyElements[j].ResultAmpl_PhaseElements[i].Phase_degree, 2).ToString();
                        }
                        else
                        {
                            Line = string.Format("{0}\t{1}", Line, Math.Round(Result.FrequencyElements[j].ResultAmpl_PhaseElements[i].Ampl_dB, 2));
                            Line2 = string.Format("{0}\t{1}", Line2, Math.Round(Result.FrequencyElements[j].ResultAmpl_PhaseElements[i].Phase_degree, 2));
                        }
                    }

                    if (i == 0)
                    {
                        RTB.AppendText(string.Format("{1}\t {0} \n", FreqLine, SpesialText));
                        RTB2.AppendText(string.Format("{1}\t {0} \n", FreqLine, SpesialText));
                    }

                    RTB.AppendText(string.Format("{0}\t{1}\n", Math.Round(Result.FrequencyElements[0].ResultAmpl_PhaseElements[i].Cordinate, 2), Line));
                    RTB2.AppendText(string.Format("{0}\t{1}\n", Math.Round(Result.FrequencyElements[0].ResultAmpl_PhaseElements[i].Cordinate, 2), Line2));
                }


                

                RTB.SaveFile(string.Format("{0} ampl.txt", SaveFilePath), RichTextBoxStreamType.PlainText);
                RTB2.SaveFile(string.Format("{0} phase.txt", SaveFilePath), RichTextBoxStreamType.PlainText);
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(SaveFilePath);
                dir.Create();

                foreach (FrequencyElementClass fe in Result.FrequencyElements)
                {
                    try
                    {
                        SaveFrequencyElementToTextFile(SaveFilePath, fe);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(String.Format("Невозможно сохранить {0}: {1}", fe, ex.Message));
                    }
                }
            }
        }

        public static void SaveResultToTextFiles(string SaveFolderPath, IResultType_MAIN Result, bool InOneFile = false)
        {
            string SaveFolderPathResult = string.Format("{0}\\{1}", SaveFolderPath, Result.ToString());
            string SaveFilePath = string.Format("{0}\\{1}.txt", SaveFolderPathResult, Result.ToString());


            DirectoryInfo dir = new DirectoryInfo(SaveFolderPathResult);
            dir.Create();

            RichTextBox RTB = new RichTextBox();
            RTB.AppendText(string.Format("{0}\n", Result));
            RTB.AppendText(string.Format("{0}\n", Result.MainOptions.Date));
            RTB.AppendText(string.Format("ИА: {0}\n", Result.Antenn));
            RTB.AppendText(string.Format("ТА: {0}\n", Result.Zond));

            RTB.SaveFile(SaveFilePath, RichTextBoxStreamType.PlainText);


            //делаем интерполяцию для последущего вывода результатов
            double Step;
            double Start;
            double Stop;
            ResultType_MAINClass.GetCoordinatForInterpolation(Result.MainOptions, out Start, out Stop, out Step);
            

            for(int i=0;i< Result.PolarizationElements.Count;i++)
            {
                if(Result.PolarizationElements[i].Polarization!=SelectedPolarizationEnum.Sum)  //в суммарных уже есть интерполяция
                {
                    for(int j=0;j< Result.PolarizationElements[i].FrequencyElements.Count;j++)
                    {
                        FrequencyElementClass tempMain = FrequencyElementClass.InterpolationByStep(Result.PolarizationElements[i].FrequencyElements[j], Step, Start, Stop);
                        Result.PolarizationElements[i].FrequencyElements[j] = tempMain;
                    }
                }

            }


            



            foreach (PolarizationElementClass pol in Result.PolarizationElements)
            {
                SavePolarizationElementToTextFile(SaveFolderPathResult, pol, InOneFile,Result.MainOptions.Parameters.StartTower_Y.ToString(), Result.id.ToString());
            }
        }


        public static void SaveResultToTextFileInFolder(IWin32Window window, IResultType_MAIN Result)
        {
            DialogResult dr = MessageBox.Show(window, string.Format("Экспортировать в 1 файл? \n Yes - в один \n No - в несколько"), "Экспорт", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (dr == DialogResult.Yes || dr == DialogResult.No)
            {
                FolderBrowserDialog fd = new FolderBrowserDialog();
                fd.Description = "Выберите папку для сохранения";

                if (fd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (dr == DialogResult.Yes)
                            SaveResultToTextFiles(fd.SelectedPath, Result, true);
                        else if (dr == DialogResult.No)
                            SaveResultToTextFiles(fd.SelectedPath, Result, false);

                        try
                        {
                            System.Diagnostics.Process.Start(fd.SelectedPath);
                        }
                        catch { }
                        MessageBox.Show(window, "Экспорт выполнен успешно", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(window, string.Format("Ошибка экспорта: \n{0}", ex.Message), "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

       public static void SaveResultToTextFileInFolder(IResultType_MAIN Result)
       {
           SaveResultToTextFileInFolder(null, Result);
       }

       #endregion


       private void buttonTXT_Click(object sender, EventArgs e)
        {
            if (ResultForExport != null)
            {
                SaveResultToTextFileInFolder(this, ResultForExport);
            }
            else
            {
                MessageBox.Show(this, "Экспорт невозможен. Отсутствует объект для экспорта", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

       private void buttonXML_Click(object sender, EventArgs e)
       {
           if (ResultForExport != null)
           {
               SaveResultToXMLFileInFolder(this, ResultForExport);
           }
           else
           {
               MessageBox.Show(this, "Экспорт невозможен. Отсутствует объект для экспорта", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Warning);
           }
       }

       private void buttonXLSX_Click(object sender, EventArgs e)
       {
           if (ResultForExport != null)
           {
               SaveResultToXLSXFileInFolder(this, ResultForExport);
           }
           else
           {
               MessageBox.Show(this, "Экспорт невозможен. Отсутствует объект для экспорта", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Warning);
           }
       }

       #region функции загрузки и сохранения в XML


       public static void SaveResultToXMLFile(IResultType_MAIN SaveObject, string SaveName)
       {
           UniversalXmlSerializer.UnivrsalXmlSerializerClass.SaveXML(SaveObject, SaveName);
       }

       public static void SaveResultToXMLFileInFolder(IWin32Window window, IResultType_MAIN Result)
       {
           SaveFileDialog fd = new SaveFileDialog();

           fd.Title = "Выберите куда экспортировать";
           fd.FileName = Result.ToString();
           fd.Filter = "XML файлы|*.xml";

           if (fd.ShowDialog() == DialogResult.OK)
           {
               try
               {
                   SaveResultToXMLFile(Result, fd.FileName);


                   //try
                   //{
                    //   System.Diagnostics.Process.Start(fd.FileName);
                   //}
                   //catch { }

                   MessageBox.Show(window, "Экспорт выполнен успешно", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
               catch (Exception ex)
               {
                   MessageBox.Show(window, string.Format("Ошибка экспорта: \n{0}", ex.Message), "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Error);
               }
           }
       }

       public static void SaveResultToXMLFileInFolder(IWin32Window window, List<IResultType_MAIN> Results)
       {
           FolderBrowserDialog fd = new FolderBrowserDialog();
           fd.Description = "Выберите папку для сохранения";


           if (fd.ShowDialog() == DialogResult.OK)
           {
               try
               {
                   string SaveFolderPath = string.Format("{0}\\{1}", fd.SelectedPath, Results[0].Antenn.ToString());
                   DirectoryInfo dir = new DirectoryInfo(SaveFolderPath);
                   dir.Create();
                   string ErrTexts = "";

                   foreach (IResultType_MAIN Result in Results)
                   {
                       string SaveFilePath = string.Format("{0}\\{1}.xml", SaveFolderPath, Result.ToString());

                       try
                       {
                           SaveResultToXMLFile(Result, SaveFilePath);
                       }
                       catch (Exception ex)
                       {
                           ErrTexts += string.Format("Ошибка экспорта в файле {1}: {0}\n", ex.Message, SaveFilePath);
                       }
                   }
                   try
                   {
                       if (ErrTexts != "")
                           MessageBox.Show(window, string.Format("Экспорт завршён с сообщениями:\n{0}", ErrTexts), "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                       System.Diagnostics.Process.Start(SaveFolderPath);
                   }
                   catch { }
               }
               catch (Exception ex)
               {
                   MessageBox.Show(window, string.Format("Ошибка экспорта: \n{0}", ex.Message), "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Error);
               }
           }
       }

       public static void SaveResultToXMLFileInFolder(IResultType_MAIN Result)
       {
           SaveResultToXMLFileInFolder(null, Result);
       }


      
       public static ResultType_MAINClass LoadResultXMLFile(string FileName)
       {
           object ret=null;

           if (UniversalXmlSerializer.UnivrsalXmlSerializerClass.LoadXML<ResultTypeClassДН>(FileName, out ret))
               return ret as ResultType_MAINClass;

           if (UniversalXmlSerializer.UnivrsalXmlSerializerClass.LoadXML<ResultTypeClassКУ>(FileName, out ret))
               return ret as ResultType_MAINClass;

           if (UniversalXmlSerializer.UnivrsalXmlSerializerClass.LoadXML<ResultTypeClassСДНМ>(FileName, out ret))
               return ret as ResultType_MAINClass;

           if (UniversalXmlSerializer.UnivrsalXmlSerializerClass.LoadXML<ResultTypeПХ>(FileName, out ret))
               return ret as ResultType_MAINClass;

           if (UniversalXmlSerializer.UnivrsalXmlSerializerClass.LoadXML<ResultTypeClassUnion>(FileName, out ret))
               return ret as ResultType_MAINClass;

           if (UniversalXmlSerializer.UnivrsalXmlSerializerClass.LoadXML<ResultType_MAINClass>(FileName, out ret))
               return ret as ResultType_MAINClass;

           return ret as ResultType_MAINClass;
       }

       public static IResultType_MAIN LoadResultXMLFileInFolder(IWin32Window window)
       {
           IResultType_MAIN Result = null;
           string ErrorText = "";

           OpenFileDialog fd = new OpenFileDialog();

           fd.Title = "Выберите файл для загрузки";
           fd.Filter = "XML файлы|*.xml";

           if (fd.ShowDialog() == DialogResult.OK)
           {
               try
               {
                   Result = LoadResultXMLFile(fd.FileName);
               }
               catch (Exception ex)
               {
                   ErrorText = ex.Message;
               }
           }

           if (ErrorText != "")
           {
               MessageBox.Show(window, string.Format("Ошибка импорта: \n{0}", ErrorText), "Импорт", MessageBoxButtons.OK, MessageBoxIcon.Error);
           }

           return Result;
       }


       #endregion


        #region Функции экспорта в Exel (*.XLSX)

       public static void SaveResultToXLSXFileInFolder(IWin32Window window, IResultType_MAIN Result)
       {
           SaveFileDialog fd = new SaveFileDialog();

           fd.Title = "Выберите куда экспортировать";
           fd.FileName = Result.ToString();
           fd.Filter = "XLSX файлы|*.xlsx";

           if (fd.ShowDialog() == DialogResult.OK)
           {
               try
               {
                   SaveResultToXLSXFile(Result, fd.FileName);


                   try
                   {
                       System.Diagnostics.Process.Start(fd.FileName);
                   }
                   catch { }

                  // MessageBox.Show(window, "Экспорт выполнен успешно", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
               }
               catch (Exception ex)
               {
                   MessageBox.Show(window, string.Format("Ошибка экспорта: \n{0}", ex.Message), "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Error);
               }
           }
       }

       public static void SaveResultToXLSXFileInFolder(IResultType_MAIN Result)
       {
           SaveResultToXLSXFileInFolder(null, Result);
       }

       protected static DB_Forms.Check_Options_XLSXForm CheckOptionsForXLSX = new DB_Forms.Check_Options_XLSXForm();

        public static void SaveResultToXLSXFileInFolder(IWin32Window window, List<IResultType_MAIN> Results)
       {
           FolderBrowserDialog fd = new FolderBrowserDialog();
           fd.Description = "Выберите папку для сохранения";


           if (fd.ShowDialog() == DialogResult.OK)
           {
                if (CheckOptionsForXLSX.ShowDialog() == DialogResult.OK)
                {

                    try
                    {
                        string SaveFolderPath = string.Format("{0}\\{1}", fd.SelectedPath, Results[0].Antenn.ToString());
                        DirectoryInfo dir = new DirectoryInfo(SaveFolderPath);
                        dir.Create();
                        string ErrTexts = "";

                        foreach (IResultType_MAIN Result in Results)
                        {
                            string SaveFilePath = string.Format("{0}\\{1}.xlsx", SaveFolderPath, Result.ToString());

                            try
                            {
                                SaveResultToXLSXFile(Result, SaveFilePath);
                            }
                            catch (Exception ex)
                            {
                                ErrTexts += string.Format("Ошибка экспорта в файле {1}: {0}\n", ex.Message, SaveFilePath);
                            }
                        }
                        try
                        {
                            if (ErrTexts != "")
                                MessageBox.Show(window, string.Format("Экспорт завршён с сообщениями:\n{0}", ErrTexts), "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            System.Diagnostics.Process.Start(SaveFolderPath);
                        }
                        catch { }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(window, string.Format("Ошибка экспорта: \n{0}", ex.Message), "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
           }
       }

       public static void SaveResultToXLSXFile(IResultType_MAIN SaveObject, string SaveName)
       {

           #region Создаем новый документ и настраиваем его
           SpreadsheetDocument document = SpreadsheetDocument.Create(SaveName, SpreadsheetDocumentType.Workbook);

           WorkbookPart workbookPart = document.AddWorkbookPart();
           workbookPart.Workbook = new Workbook();

           FileVersion fv = new FileVersion();
           fv.ApplicationName = "Microsoft Office Excel";
           WorkbookStylesPart wbsp = workbookPart.AddNewPart<WorkbookStylesPart>();

           // Добавляем в документ набор стилей
           wbsp.Stylesheet =OpenXML_DocumentCreatorClass.GenerateStyleSheet();
           wbsp.Stylesheet.Save();
           #endregion


           #region тело документа
           uint[] WidthColumns = { 30, 30};
           SheetData sheetData = OpenXML_DocumentCreatorClass.CreateSheet(workbookPart, "Опции измерения", WidthColumns);

           Row row = new Row() { RowIndex = 1 };
           sheetData.Append(row);
           OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Название", CellValues.String, 0);
           OpenXML_DocumentCreatorClass.InsertCell(row, 2, SaveObject.ToString(), CellValues.String, 0);

           row = new Row() { RowIndex = 2 };
           sheetData.Append(row);
           OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Дата", CellValues.String, 0);
           OpenXML_DocumentCreatorClass.InsertCell(row, 2, SaveObject.MainOptions.Date.ToString(), CellValues.String, 0);

           row = new Row() { RowIndex = 3 };
           sheetData.Append(row);
           OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Испытуемая антенна", CellValues.String, 0);
           OpenXML_DocumentCreatorClass.InsertCell(row, 2, SaveObject.Antenn.ToString(), CellValues.String, 0);

           row = new Row() { RowIndex = 4 };
           sheetData.Append(row);
           OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Технологическая антенна", CellValues.String, 0);
           OpenXML_DocumentCreatorClass.InsertCell(row, 2, SaveObject.Zond.ToString(), CellValues.String, 0);

            #endregion

            #region сохраняем частоты

            foreach (PolarizationElementClass pol in SaveObject.PolarizationElements)
            {
                string PolarizationName = "";
                if (pol.Polarization != SelectedPolarizationEnum.None) PolarizationName = pol.Polarization.ToString();

                if (pol.Polarization == SelectedPolarizationEnum.Main)
                {
                    if (!CheckOptionsForXLSX.NeedMain) continue;
                }
                else if (pol.Polarization == SelectedPolarizationEnum.Cross)
                {
                    if (!CheckOptionsForXLSX.NeedCross) continue;
                }
                else if (pol.Polarization == SelectedPolarizationEnum.Sum)
                {
                    if (!CheckOptionsForXLSX.NeedSum) continue;
                }


                foreach (FrequencyElementClass freq in pol.FrequencyElements)
                {
                    string sheetName =/* SaveObject.id.ToString() + " " + PolarizationName + " " +*/freq.ToString();
                    uint[] WidthColumns2 = { 20, 20, 20 };
                    sheetData = OpenXML_DocumentCreatorClass.CreateSheet(workbookPart, sheetName, WidthColumns2);

                    UInt32 i = 1;
                    //заголовок
                    row = new Row() { RowIndex = i };
                    sheetData.Append(row);
                    OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Координата");
                    OpenXML_DocumentCreatorClass.InsertCell(row, 2, "Амплитуда");
                    OpenXML_DocumentCreatorClass.InsertCell(row, 3, "Фаза");

                    if(CheckOptionsForXLSX.NeedMaxДН)
                    {
                        OpenXML_DocumentCreatorClass.InsertCell(row, 4, "");
                        OpenXML_DocumentCreatorClass.InsertCell(row, 5, "Max ДН");
                        OpenXML_DocumentCreatorClass.InsertCell(row, 6, freq.CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности);
                        OpenXML_DocumentCreatorClass.InsertCell(row, 7, freq.CalculationResults.Направление_максимума_диаграммы_направленности);
                    }

                    i++;

                    foreach (ResultElementClass el in freq.ResultAmpl_PhaseElements)
                    {
                        row = new Row() { RowIndex = i };
                        sheetData.Append(row);

                        string tt = el.Cordinate.ToString();

                        string t2 = Convert.ToString(el.Cordinate);
                        string t3 = Convert.ToDecimal(el.Cordinate).ToString();




                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, el.Cordinate);
                        OpenXML_DocumentCreatorClass.InsertCell(row, 2, el.Ampl_dB);
                        OpenXML_DocumentCreatorClass.InsertCell(row, 3, el.Phase_degree);


                        if (i == 2 && CheckOptionsForXLSX.NeedШирина_ДН)
                        {
                            OpenXML_DocumentCreatorClass.InsertCell(row, 4, "");
                            OpenXML_DocumentCreatorClass.InsertCell(row, 5, "Ширина ДН -3 ДБ");
                            OpenXML_DocumentCreatorClass.InsertCell(row, 6, freq.CalculationResults.Ширина_диаграммы_направленности_по_половине_мощности);
                        }

                        i++;
                    }


                }




            }

           #endregion

           
           workbookPart.Workbook.Save();
           document.Close();
       }


       public static void SaveResultToXLSX_ONE_FileInFolder(IWin32Window window, List<IResultType_MAIN> Results)
       {
           SaveFileDialog fd = new SaveFileDialog();

           fd.Title = "Выберите куда экспортировать";
           fd.FileName = Results[0].Antenn.ToString();
           fd.Filter = "XLSX файлы|*.xlsx";

           if (fd.ShowDialog() == DialogResult.OK)
           {
               if (Results[0] is IResultType_СДНМ)
               {
                   try
                   {
                       #region создание xlsx

                       #region Создаем новый документ и настраиваем его
                       SpreadsheetDocument document = SpreadsheetDocument.Create(fd.FileName, SpreadsheetDocumentType.Workbook);

                       WorkbookPart workbookPart = document.AddWorkbookPart();
                       workbookPart.Workbook = new Workbook();

                       FileVersion fv = new FileVersion();
                       fv.ApplicationName = "Microsoft Office Excel";
                       WorkbookStylesPart wbsp = workbookPart.AddNewPart<WorkbookStylesPart>();

                       // Добавляем в документ набор стилей
                       wbsp.Stylesheet = OpenXML_DocumentCreatorClass.GenerateStyleSheet();
                       wbsp.Stylesheet.Save();
                       #endregion

                       #region первый саодный лист в таблице

                       uint[] WidthColumns = { 10, 10, 10, 10, 20, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
                       SheetData sheetData = OpenXML_DocumentCreatorClass.CreateSheet(workbookPart, "Сводная таблица", WidthColumns);

                       bool fistAdd = true;



                       for (int j = 0; j < Results.Count; j++)
                       {
                           if (Results[j] is IResultType_СДНМ)
                           {
                               IResultType_СДНМ restemp = Results[j] as IResultType_СДНМ;

                               Row row = new Row();
                               Row row2 = new Row();
                               if (fistAdd)
                               {
                                   row = new Row() { RowIndex = 1 };
                                   sheetData.Append(row);

                                   OpenXML_DocumentCreatorClass.InsertCell(row, 1, "id");
                                   OpenXML_DocumentCreatorClass.InsertCell(row, 2, "port №");
                                   OpenXML_DocumentCreatorClass.InsertCell(row, 3, "Plan");
                                   OpenXML_DocumentCreatorClass.InsertCell(row, 4, "TB deg");
                                   OpenXML_DocumentCreatorClass.InsertCell(row, 5, "Описание");


                                   row2 = new Row() { RowIndex = 2 };
                                   sheetData.Append(row2);
                                   OpenXML_DocumentCreatorClass.InsertCell(row2, 1, "");
                                   OpenXML_DocumentCreatorClass.InsertCell(row2, 1, "");
                                   OpenXML_DocumentCreatorClass.InsertCell(row2, 1, "");
                                   OpenXML_DocumentCreatorClass.InsertCell(row2, 1, "");
                                   OpenXML_DocumentCreatorClass.InsertCell(row2, 1, "");
                               }


                               Row row1 = new Row() { RowIndex = Convert.ToUInt32(j + 4) };
                               sheetData.Append(row1);
                               OpenXML_DocumentCreatorClass.InsertCell(row1, 1, restemp.id);
                               OpenXML_DocumentCreatorClass.InsertCell(row1, 2, "");

                               string adtext = "X3";
                               if (restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Азимут) adtext = "Az";
                               else if (restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Меридиан) adtext = "Mer";
                               OpenXML_DocumentCreatorClass.InsertCell(row1, 3, adtext);
                               OpenXML_DocumentCreatorClass.InsertCell(row1, 4, "");
                               OpenXML_DocumentCreatorClass.InsertCell(row1, 5, restemp.MainOptions.Descriptions);

                               for (int i = 0; i < restemp.SUM_Polarization.FrequencyElements.Count; i++)
                               {
                                   if (fistAdd) //заполняем шапку
                                   {
                                       OpenXML_DocumentCreatorClass.InsertCell(row, i * 2 + 6, "Gain, dB");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, i * 2 + 7, "BP, deg");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, i * 2 + 8, "Max, deg");


                                       OpenXML_DocumentCreatorClass.InsertCell(row2, i * 2 + 6, restemp.SUM_Polarization.FrequencyElements[i].Frequency);
                                       OpenXML_DocumentCreatorClass.InsertCell(row2, i * 2 + 7, restemp.SUM_Polarization.FrequencyElements[i].Frequency);
                                       OpenXML_DocumentCreatorClass.InsertCell(row2, i * 2 + 8, restemp.SUM_Polarization.FrequencyElements[i].Frequency);
                                   }

                                   OpenXML_DocumentCreatorClass.InsertCell(row1, i * 2 + 6, restemp.SUM_Polarization.FrequencyElements[i].CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности);
                                   OpenXML_DocumentCreatorClass.InsertCell(row1, i * 2 + 7, restemp.SUM_Polarization.FrequencyElements[i].CalculationResults.Ширина_диаграммы_направленности_по_половине_мощности);
                                   OpenXML_DocumentCreatorClass.InsertCell(row1, i * 2 + 8, restemp.SUM_Polarization.FrequencyElements[i].CalculationResults.Направление_максимума_диаграммы_направленности);
                               }


                               fistAdd = false;
                           }


                       }

                       #endregion

                       #region второй лист неоднородности

                       sheetData = OpenXML_DocumentCreatorClass.CreateSheet(workbookPart, "Неоднородность", WidthColumns);

                       Row rowN = new Row() { RowIndex = 1 };
                       sheetData.Append(rowN);
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Частота, МГц");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Неоднородность, дБ");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Координата");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Порт, TB № имерения минимального");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Значение минимального");

                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Порт, TB № имерения макс");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Значение макс");


                       List<List<FrequencyElementDescriptionClass>> Azimut = new List<List<FrequencyElementDescriptionClass>>();
                       List<List<FrequencyElementDescriptionClass>> Meridian = new List<List<FrequencyElementDescriptionClass>>();

                       List<List<FrequencyElementDescriptionClass>> Azimut2 = new List<List<FrequencyElementDescriptionClass>>();


                       #region разбиваем по рабочим портам и частотам

                       for (int j = 0; j < Results.Count; j++)
                       {
                           if (Results[j] is IResultType_СДНМ)
                           {
                               IResultType_СДНМ restemp = Results[j] as IResultType_СДНМ;

                               int portN = -1;
                               int tbN = -1;

                               int startF = 0;
                               int stopF = restemp.Main_Polarization.FrequencyElements.Count;

                               SaverResultForm.DecodingDescription(restemp, out portN, out tbN, out startF, out stopF);


                               for (int s = startF; s <= stopF; s++)
                               {
                                   //делаем интерполяцию для последущего сложения результатов
                                   double Step;
                                   double Start;
                                   double Stop;
                                   ResultType_MAINClass.GetCoordinatForInterpolation(restemp.MainOptions, out Start, out Stop, out Step);
                                   Step = 0.5;
                                   FrequencyElementClass tempMain = FrequencyElementClass.InterpolationByStep(restemp.Main_Polarization.FrequencyElements[s], Step, Start, Stop);


                                   FrequencyElementClass freq = tempMain;

                                   //для азимутальных измерений
                                   if (restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Азимут)
                                   {
                                       bool Find = false;
                                       for (int z = 0; z < Azimut.Count; z++)
                                       {
                                           List<FrequencyElementDescriptionClass> ListFreq = Azimut[z];

                                           if (ListFreq[0].Freq.Frequency == freq.Frequency)
                                           {
                                               ListFreq.Add(new FrequencyElementDescriptionClass(portN, tbN, restemp.id, freq));

                                               //дублируем ещё в один массив
                                               Azimut2[z].Add(new FrequencyElementDescriptionClass(portN, tbN, restemp.id, freq));

                                               Find = true;
                                               break;
                                           }
                                       }

                                       if (!Find)
                                       {
                                           //если не нашли в массиве, то добавляем новый
                                           List<FrequencyElementDescriptionClass> TempFreq = new List<FrequencyElementDescriptionClass>();
                                           TempFreq.Add(new FrequencyElementDescriptionClass(portN, tbN, restemp.id, freq));
                                           Azimut.Add(TempFreq);

                                           //дублируем ещё в один массив
                                           List<FrequencyElementDescriptionClass> TempFreq2 = new List<FrequencyElementDescriptionClass>();
                                           TempFreq2.Add(new FrequencyElementDescriptionClass(portN, tbN, restemp.id, freq));
                                           Azimut2.Add(TempFreq2);
                                       }
                                   }

                                       //для меридианальных измерений
                                   else if (restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Меридиан)
                                   {
                                       bool Find = false;
                                       foreach (List<FrequencyElementDescriptionClass> ListFreq in Meridian)
                                       {
                                           if (ListFreq[0].Freq.Frequency == freq.Frequency)
                                           {
                                               ListFreq.Add(new FrequencyElementDescriptionClass(portN, tbN, restemp.id, freq));
                                               Find = true;
                                           }
                                       }

                                       if (!Find)
                                       {
                                           //если не нашли в массиве, то добавляем новый
                                           List<FrequencyElementDescriptionClass> TempFreq = new List<FrequencyElementDescriptionClass>();
                                           TempFreq.Add(new FrequencyElementDescriptionClass(portN, tbN, restemp.id, freq));
                                           Meridian.Add(TempFreq);
                                       }
                                   }

                               }
                           }
                       }

                       #endregion




                       int rowCount = 0;

                       #region По меридиану
                       for (int z = 0; z < Meridian.Count; z++)
                       {
                           //выбрали массив на одной частоте
                           List<FrequencyElementDescriptionClass> TempFreqMass = Meridian[z];

                           double LeftMax = double.MaxValue;
                           double RightMax = double.MinValue;

                           foreach (FrequencyElementDescriptionClass TempFreq in TempFreqMass)
                           {
                               //перебираем данные частот на разных измерениях
                               List<PointDouble> dataAmplMain = CalculationResultsClass.GetListPoint_Ampl_FromFrequencyElement(TempFreq.Freq);

                               PointDouble Max;
                               PointDouble Left05;
                               PointDouble Right05;
                               PointDouble Max2;
                               int MaxIndex;
                               int LeftIndex;
                               int RightIndex;
                               double winghDN;

                               PointDouble LeftLepestok;
                               PointDouble RightLepestok;

                               Max = CalculationClass.CalculationMainMax(dataAmplMain, out Left05, out Right05, out Max2, 1, out MaxIndex, out LeftIndex, out RightIndex, out winghDN, out LeftLepestok, out RightLepestok, 0.2d);


                               if (Left05.X < LeftMax) LeftMax = Left05.X;
                               if (Right05.X > RightMax) RightMax = Right05.X;

                           }



                           //прошли все частоты, теперь режем диаграммы по максимальными растояниям от максимума ДН

                           foreach (FrequencyElementDescriptionClass TempFreq in TempFreqMass)
                           {
                               FrequencyElementClass tempMain = FrequencyElementClass.InterpolationByStep(TempFreq.Freq, 0.5d, LeftMax, RightMax);
                               //нормализацию сюда
                               TempFreq.Freq = tempMain;
                           }

                           double MaxDistange = double.MinValue;
                           double MaxTop = double.MinValue;
                           double MaxBottom = double.MaxValue;
                           string DiscriptionTop = "";
                           string DiscriptionBottom = "";
                           string Andgle = "";

                           for (int i = 0; i < TempFreqMass[0].Freq.ResultAmpl_PhaseElements.Count; i++)
                           {
                               double MaxTopTemp = double.MinValue;
                               double MaxBottomTemp = double.MaxValue;
                               string DiscriptionTopTemp = "";
                               string DiscriptionBottomTemp = "";


                               for (int j = 0; j < TempFreqMass.Count; j++)
                               {
                                   if (MaxTopTemp < TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxTopTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionTopTemp = TempFreqMass[j].ToString();
                                   }
                                   if (MaxBottomTemp > TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxBottomTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionBottomTemp = TempFreqMass[j].ToString();
                                   }
                               }

                               if (MaxTopTemp - MaxBottomTemp > MaxDistange)
                               {
                                   MaxDistange = MaxTopTemp - MaxBottomTemp;
                                   MaxTop = MaxTopTemp;
                                   MaxBottom = MaxBottomTemp;

                                   Andgle = TempFreqMass[0].Freq.ResultAmpl_PhaseElements[i].Cordinate.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                                   DiscriptionBottom = DiscriptionBottomTemp;
                                   DiscriptionTop = DiscriptionTopTemp;
                               }
                           }

                           //записываем результат в эксельку

                           rowN = new Row() { RowIndex = Convert.ToUInt32(rowCount + 2) };
                           rowCount++;
                           sheetData.Append(rowN);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "MerV1");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, TempFreqMass[0].Freq.Frequency);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxDistange);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, Andgle);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionTop);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxTop);
                       }
                       #endregion



                       #region По азимуту
                       for (int z = 0; z < Azimut.Count; z++)
                       {
                           //выбрали массив на одной частоте
                           List<FrequencyElementDescriptionClass> TempFreqMass = Azimut[z];

                           double LeftMax = double.MaxValue;
                           double RightMax = double.MinValue;

                           foreach (FrequencyElementDescriptionClass TempFreq in TempFreqMass)
                           {
                               //перебираем данные частот на разных измерениях




                               List<PointDouble> dataAmplMain = CalculationResultsClass.GetListPoint_Ampl_FromFrequencyElement(TempFreq.Freq);

                               PointDouble Max;
                               PointDouble Left05;
                               PointDouble Right05;
                               PointDouble Max2;
                               int MaxIndex;
                               int LeftIndex;
                               int RightIndex;
                               double winghDN;

                               PointDouble LeftLepestok;
                               PointDouble RightLepestok;

                               Max = CalculationClass.CalculationMainMax(dataAmplMain, out Left05, out Right05, out Max2, 1, out MaxIndex, out LeftIndex, out RightIndex, out winghDN, out LeftLepestok, out RightLepestok, 0.2d);


                               if (Left05.X < LeftMax) LeftMax = Left05.X;
                               if (Right05.X > RightMax) RightMax = Right05.X;

                           }

                           //прошли все частоты, теперь режем диаграммы по максимальными растояниям от максимума ДН

                           foreach (FrequencyElementDescriptionClass TempFreq in TempFreqMass)
                           {
                               FrequencyElementClass tempMain = FrequencyElementClass.InterpolationByStep(TempFreq.Freq, 0.5d, LeftMax, RightMax);
                               TempFreq.Freq = tempMain;
                           }

                           double MaxDistange = double.MinValue;
                           double MaxTop = double.MinValue;
                           double MaxBottom = double.MaxValue;
                           string DiscriptionTop = "";
                           string DiscriptionBottom = "";
                           string Andgle = "";

                           for (int i = 0; i < TempFreqMass[0].Freq.ResultAmpl_PhaseElements.Count; i++)
                           {
                               double MaxTopTemp = double.MinValue;
                               double MaxBottomTemp = double.MaxValue;
                               string DiscriptionTopTemp = "";
                               string DiscriptionBottomTemp = "";


                               for (int j = 0; j < TempFreqMass.Count; j++)
                               {
                                   if (MaxTopTemp < TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxTopTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionTopTemp = TempFreqMass[j].ToString();
                                   }
                                   if (MaxBottomTemp > TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxBottomTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionBottomTemp = TempFreqMass[j].ToString();
                                   }
                               }

                               if (MaxTopTemp - MaxBottomTemp > MaxDistange)
                               {
                                   MaxDistange = MaxTopTemp - MaxBottomTemp;
                                   MaxTop = MaxTopTemp;
                                   MaxBottom = MaxBottomTemp;

                                   Andgle = TempFreqMass[0].Freq.ResultAmpl_PhaseElements[i].Cordinate.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                                   DiscriptionBottom = DiscriptionBottomTemp;
                                   DiscriptionTop = DiscriptionTopTemp;
                               }
                           }

                           //записываем результат в эксельку

                           rowN = new Row() { RowIndex = Convert.ToUInt32(rowCount + 3) };
                           rowCount++;
                           sheetData.Append(rowN);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "AzV1");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, TempFreqMass[0].Freq.Frequency);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxDistange);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, Andgle);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionTop);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxTop);
                       }
                       #endregion

                       #region По азимуту 2вар
                       for (int z = 0; z < Azimut2.Count; z++)
                       {
                           //выбрали массив на одной частоте
                           List<FrequencyElementDescriptionClass> TempFreqMass = Azimut2[z];

                           double LeftMax = double.MaxValue;
                           double RightMax = double.MinValue;

                           if (TempFreqMass[0].Freq.Frequency < 900 || TempFreqMass[0].Freq.Frequency < 2100 && TempFreqMass[0].Freq.Frequency >= 1800)
                           {
                               LeftMax = 180d - 67d / 2d;
                               RightMax = 180d + 67d / 2d;
                           }
                           else if (TempFreqMass[0].Freq.Frequency >= 900 && TempFreqMass[0].Freq.Frequency < 1800 || TempFreqMass[0].Freq.Frequency >= 2300)
                           {
                               LeftMax = 180d - 64d / 2d;
                               RightMax = 180d + 64d / 2d;
                           }
                           else if (TempFreqMass[0].Freq.Frequency >= 2100 && TempFreqMass[0].Freq.Frequency < 2300)
                           {
                               LeftMax = 180d - 65d / 2d;
                               RightMax = 180d + 65d / 2d;
                           }

                           //прошли все частоты, теперь режем диаграммы по максимальными растояниям от максимума ДН

                           foreach (FrequencyElementDescriptionClass TempFreq in TempFreqMass)
                           {
                               FrequencyElementClass tempMain = FrequencyElementClass.InterpolationByStep(TempFreq.Freq, 0.5d, LeftMax, RightMax);
                               TempFreq.Freq = tempMain;
                           }

                           double MaxDistange = double.MinValue;
                           double MaxTop = double.MinValue;
                           double MaxBottom = double.MaxValue;
                           string DiscriptionTop = "";
                           string DiscriptionBottom = "";
                           string Andgle = "";

                           for (int i = 0; i < TempFreqMass[0].Freq.ResultAmpl_PhaseElements.Count; i++)
                           {
                               double MaxTopTemp = double.MinValue;
                               double MaxBottomTemp = double.MaxValue;
                               string DiscriptionTopTemp = "";
                               string DiscriptionBottomTemp = "";


                               for (int j = 0; j < TempFreqMass.Count; j++)
                               {
                                   if (MaxTopTemp < TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxTopTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionTopTemp = TempFreqMass[j].ToString();
                                   }
                                   if (MaxBottomTemp > TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxBottomTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionBottomTemp = TempFreqMass[j].ToString();
                                   }
                               }

                               if (MaxTopTemp - MaxBottomTemp > MaxDistange)
                               {
                                   MaxDistange = MaxTopTemp - MaxBottomTemp;
                                   MaxTop = MaxTopTemp;
                                   MaxBottom = MaxBottomTemp;

                                   Andgle = TempFreqMass[0].Freq.ResultAmpl_PhaseElements[i].Cordinate.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                                   DiscriptionBottom = DiscriptionBottomTemp;
                                   DiscriptionTop = DiscriptionTopTemp;
                               }
                           }

                           //записываем результат в эксельку

                           rowN = new Row() { RowIndex = Convert.ToUInt32(rowCount + 4) };
                           rowCount++;
                           sheetData.Append(rowN);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "AzV2");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, TempFreqMass[0].Freq.Frequency);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxDistange);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, Andgle);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionTop);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxTop);
                       }
                       #endregion

                       #region Разбивка по связанным портам

                       List<List<FrequencyElementDescriptionClass>> Azimut12 = new List<List<FrequencyElementDescriptionClass>>();
                       List<List<FrequencyElementDescriptionClass>> Azimut34 = new List<List<FrequencyElementDescriptionClass>>();
                       List<List<FrequencyElementDescriptionClass>> Azimut56 = new List<List<FrequencyElementDescriptionClass>>();
                       List<List<FrequencyElementDescriptionClass>> Azimut78 = new List<List<FrequencyElementDescriptionClass>>();
                        List<List<FrequencyElementDescriptionClass>> Azimut910 = new List<List<FrequencyElementDescriptionClass>>();
                        List<List<FrequencyElementDescriptionClass>> Azimut1112 = new List<List<FrequencyElementDescriptionClass>>();



                        //используем уже порезанные массивы
                        for (int z = 0; z < Azimut2.Count; z++)
                       {
                           //выбрали массив на одной частоте
                           List<FrequencyElementDescriptionClass> TempFreqMass = Azimut2[z];

                           for (int i = 0; i < TempFreqMass.Count; i++)
                           {
                               switch (TempFreqMass[i].Port)
                               {
                                   case 1:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut12, TempFreqMass[i]);
                                           break;
                                       }
                                   case 2:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut12, TempFreqMass[i]);
                                           break;
                                       }
                                   case 3:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut34, TempFreqMass[i]);
                                           break;
                                       }
                                   case 4:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut34, TempFreqMass[i]);
                                           break;
                                       }
                                   case 5:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut56, TempFreqMass[i]);
                                           break;
                                       }
                                   case 6:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut56, TempFreqMass[i]);
                                           break;
                                       }
                                   case 7:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut78, TempFreqMass[i]);
                                           break;
                                       }
                                   case 8:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut78, TempFreqMass[i]);
                                           break;
                                       }
                                    case 9:
                                        {
                                            AddToFrequencyElementDescriptionList(ref Azimut910, TempFreqMass[i]);
                                            break;
                                        }
                                    case 10:
                                        {
                                            AddToFrequencyElementDescriptionList(ref Azimut910, TempFreqMass[i]);
                                            break;
                                        }
                                    case 11:
                                        {
                                            AddToFrequencyElementDescriptionList(ref Azimut1112, TempFreqMass[i]);
                                            break;
                                        }
                                    case 12:
                                        {
                                            AddToFrequencyElementDescriptionList(ref Azimut1112, TempFreqMass[i]);
                                            break;
                                        }

                                    default:
                                       {
                                            System.Diagnostics.Trace.TraceWarning("Порт не участвует в расчетах Ошибочный номер порта?");
                                           break;
                                       }
                               }
                           }
                       }



                       #endregion

                       #region расчет по связанным портам
                       //скидываем все в один массив
                       List<List<FrequencyElementDescriptionClass>> AzimutPortFull = new List<List<FrequencyElementDescriptionClass>>();
                       AzimutPortFull.AddRange(Azimut12);
                       AzimutPortFull.AddRange(Azimut34);
                       AzimutPortFull.AddRange(Azimut56);
                       AzimutPortFull.AddRange(Azimut78);
                        AzimutPortFull.AddRange(Azimut910);
                        AzimutPortFull.AddRange(Azimut1112);

                        for (int z = 0; z < AzimutPortFull.Count; z++)
                       {
                           //выбрали массив на одной частоте
                           List<FrequencyElementDescriptionClass> TempFreqMass = AzimutPortFull[z];


                           double MaxDistange = double.MinValue;
                           double MaxTop = double.MinValue;
                           double MaxBottom = double.MaxValue;
                           string DiscriptionTop = "";
                           string DiscriptionBottom = "";
                           string Andgle = "";

                           for (int i = 0; i < TempFreqMass[0].Freq.ResultAmpl_PhaseElements.Count; i++)
                           {
                               double MaxTopTemp = double.MinValue;
                               double MaxBottomTemp = double.MaxValue;
                               string DiscriptionTopTemp = "";
                               string DiscriptionBottomTemp = "";


                               for (int j = 0; j < TempFreqMass.Count; j++)
                               {
                                   if (MaxTopTemp < TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxTopTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionTopTemp = TempFreqMass[j].ToString();
                                   }
                                   if (MaxBottomTemp > TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxBottomTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionBottomTemp = TempFreqMass[j].ToString();
                                   }
                               }

                               if (MaxTopTemp - MaxBottomTemp > MaxDistange)
                               {
                                   MaxDistange = MaxTopTemp - MaxBottomTemp;
                                   MaxTop = MaxTopTemp;
                                   MaxBottom = MaxBottomTemp;

                                   Andgle = TempFreqMass[0].Freq.ResultAmpl_PhaseElements[i].Cordinate.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                                   DiscriptionBottom = DiscriptionBottomTemp;
                                   DiscriptionTop = DiscriptionTopTemp;
                               }
                           }

                           //записываем результат в эксельку

                           rowN = new Row() { RowIndex = Convert.ToUInt32(rowCount + 5) };
                           rowCount++;
                           sheetData.Append(rowN);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "AzV3");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, TempFreqMass[0].Freq.Frequency);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxDistange);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, Andgle);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionTop);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxTop);
                       }



                       #endregion

                       #endregion


                       #region добавляем значения измерений и частот

                       for (int j = 0; j < Results.Count; j++)
                       {
                           if (Results[j] is IResultType_СДНМ)
                           {
                               IResultType_СДНМ restemp = Results[j] as IResultType_СДНМ;

                               IResultType_СДНМ CrossSpesialResult = null;

                               int portN = -1;
                               int tbN = -1;

                               int startF = 0;
                               int stopF = restemp.Main_Polarization.FrequencyElements.Count;

                               SaverResultForm.DecodingDescription(restemp, out portN, out tbN, out startF, out stopF);


                               #region ищем связанные измерения по прту и градусу наклона

                               if (portN >= 0 && tbN >= 0)
                               {
                                   for (int z = 0; z < Results.Count; z++)
                                   {
                                       if (CrossSpesialResult != null) break; //выходим, тк нашли пару
                                       if (Results[z].id != restemp.id && Results[z].MainOptions.MeasurementResultType == restemp.MainOptions.MeasurementResultType)
                                       {
                                           if (Results[z] is IResultType_СДНМ)
                                           {
                                               IResultType_СДНМ tempCros = Results[z] as IResultType_СДНМ;

                                               int portNC = -1;
                                               int tbNC = -1;

                                               int startFC = -1;
                                               int stopFC = -1;

                                               SaverResultForm.DecodingDescription(tempCros, out portNC, out tbNC, out startFC, out stopFC);

                                               if (tbNC == tbN)
                                               {
                                                   #region проверка связи по портам

                                                   switch (portN)
                                                   {
                                                       case 1:
                                                           {
                                                               //связь со 2м
                                                               if (portNC == 2)
                                                               {
                                                                   CrossSpesialResult = tempCros;
                                                               }
                                                               break;
                                                           }

                                                       case 2:
                                                           {
                                                               //связь со 1м
                                                               if (portNC == 1)
                                                               {
                                                                   CrossSpesialResult = tempCros;
                                                               }
                                                               break;
                                                           }
                                                       case 3:
                                                           {
                                                               if (portNC == 4)
                                                               {
                                                                   CrossSpesialResult = tempCros;
                                                               }
                                                               break;
                                                           }
                                                       case 4:
                                                           {
                                                               if (portNC == 3)
                                                               {
                                                                   CrossSpesialResult = tempCros;
                                                               }
                                                               break;
                                                           }
                                                       case 5:
                                                           {
                                                               if (portNC == 6)
                                                               {
                                                                   CrossSpesialResult = tempCros;
                                                               }
                                                               break;
                                                           }
                                                       case 6:
                                                           {
                                                               if (portNC == 5)
                                                               {
                                                                   CrossSpesialResult = tempCros;
                                                               }
                                                               break;
                                                           }
                                                       case 7:
                                                           {
                                                               if (portNC == 8)
                                                               {
                                                                   CrossSpesialResult = tempCros;
                                                               }
                                                               break;
                                                           }
                                                       case 8:
                                                           {
                                                               if (portNC == 7)
                                                               {
                                                                   CrossSpesialResult = tempCros;
                                                               }
                                                               break;
                                                           }
                                                        case 9:
                                                            {
                                                                if (portNC == 10)
                                                                {
                                                                    CrossSpesialResult = tempCros;
                                                                }
                                                                break;
                                                            }
                                                        case 10:
                                                            {
                                                                if (portNC == 9)
                                                                {
                                                                    CrossSpesialResult = tempCros;
                                                                }
                                                                break;
                                                            }
                                                        case 11:
                                                            {
                                                                if (portNC == 12)
                                                                {
                                                                    CrossSpesialResult = tempCros;
                                                                }
                                                                break;
                                                            }
                                                        case 12:
                                                            {
                                                                if (portNC == 11)
                                                                {
                                                                    CrossSpesialResult = tempCros;
                                                                }
                                                                break;
                                                            }
                                                    }

                                                   #endregion
                                               }
                                           }
                                       }
                                   }
                               }

                               #endregion

                               if (stopF <= restemp.Main_Polarization.FrequencyElements.Count)
                               {
                                   for (int o = startF; o <= stopF; o++)
                                   {
                                       string adtext = "X3HV";
                                       if (restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Азимут) adtext = "H";
                                       else if (restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Меридиан) adtext = "V";


                                       string sheetName = string.Format("Port{0} TB{1} {2}{3}", portN.ToString(), tbN.ToString(), adtext, restemp.Main_Polarization.FrequencyElements[o].Frequency);
                                       if (sheetName == "Port3 TB7 V1838,8")
                                       { //пауза
                                       }

                                       sheetData = OpenXML_DocumentCreatorClass.CreateSheet(workbookPart, sheetName, WidthColumns);

                                       Row row = new Row() { RowIndex = 1 };
                                       sheetData.Append(row);

                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Angle");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Main");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Cross");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Parameter");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Volume");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Position");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, restemp.id);

                                       double Step;
                                       double Start;
                                       double Stop;

                                       ResultType_MAINClass.GetCoordinatForInterpolation(restemp.MainOptions, out Start, out Stop, out Step);

                                       Step = 0.5;

                                       FrequencyElementClass tempMain = FrequencyElementClass.InterpolationByStep(restemp.Main_Polarization.FrequencyElements[o], Step, Start, Stop);
                                       FrequencyElementClass tempCross = FrequencyElementClass.InterpolationByStep(restemp.Cross_Polarization.FrequencyElements[o], Step, Start, Stop);

                                       if (tempMain.ResultAmpl_PhaseElements[tempMain.ResultAmpl_PhaseElements.Count - 1].Cordinate != tempCross.ResultAmpl_PhaseElements[tempCross.ResultAmpl_PhaseElements.Count - 1].Cordinate)
                                       {

                                       }

                                       CalculationResultsClass.Calculate_DN_Part(tempMain);
                                       CalculationResultsClass.Calculate_DN_Part(tempCross);


                                       //----------------

                                       #region рассчет бокового лепестка вправо
                                       //искать будем по мэйну
                                       List<PointDouble> dataAmplMain = CalculationResultsClass.GetListPoint_Ampl_FromFrequencyElement(tempMain);

                                       PointDouble Max;
                                       PointDouble Left05;
                                       PointDouble Right05;
                                       PointDouble Max2;
                                       int MaxIndex;
                                       int LeftIndex;
                                       int RightIndex;
                                       double winghDN;

                                       PointDouble LeftLepestok;
                                       PointDouble RightLepestok;

                                       Max = CalculationClass.CalculationMainMax(dataAmplMain, out Left05, out Right05, out Max2, 1, out MaxIndex, out LeftIndex, out RightIndex, out winghDN, out LeftLepestok, out RightLepestok, 0.2d);

                                       double Grad_178_05 = Max.X + winghDN * 1.78d;
                                        double Grad_155_05 = Max.X + winghDN * 1.55d;

                                        PointDouble Find_Grad_178_05 = FindPointNear(dataAmplMain, Grad_178_05);
                                        PointDouble Find_Grad_155_05 = FindPointNear(dataAmplMain, Grad_155_05);

                                        double SpesialUBL = double.NaN;
                                       double SpesialUBLCoord = double.NaN;
                                       double SpesialUBL20 = double.NaN;
                                       double SpesialUBLCoord20 = double.NaN;


                                        #region проверка на выход за границы 1.78 ширины ДН
                                        if (Grad_178_05 >= RightLepestok.X)
                                        {
                                            // в границах все ок
                                            SpesialUBL = RightLepestok.Y - Max.Y;
                                            SpesialUBLCoord = RightLepestok.X;
                                        }
                                        else
                                        {
                                            //ищем точки рядом с найденой координатой
                                            PointDouble Find = FindPointNear(dataAmplMain, Grad_155_05);

                                            SpesialUBL = Find.Y - Max.Y;
                                            SpesialUBLCoord = Find.X;
                                        }
                                       #endregion

                                       #endregion


                                       //-----------------


                                       #region расчет front to back Ratio

                                       List<PointDouble> LeftAmplMain = null;
                                       List<PointDouble> RightAmplMain = null;

                                       CalculationClass.DeleteMassiv_By_Max(dataAmplMain, MaxIndex, out LeftAmplMain, out RightAmplMain);



                                       FrequencyElementClass FrontMain_all = new FrequencyElementClass();

                                       foreach (PointDouble point in RightAmplMain)
                                       {
                                           if (point.X <= RightAmplMain[RightAmplMain.Count - 1].X && point.X >= RightAmplMain[RightAmplMain.Count - 1].X - 30)
                                           {
                                               FrontMain_all.ResultAmpl_PhaseElements.Add(new ResultElementClass(point.X, point.Y, 0));
                                           }
                                       }


                                       foreach (PointDouble point in LeftAmplMain)
                                       {
                                           if (point.X <= LeftAmplMain[0].X + 30 && point.X >= LeftAmplMain[0].X)
                                           {
                                               FrontMain_all.ResultAmpl_PhaseElements.Add(new ResultElementClass(point.X, point.Y, 0));
                                           }
                                       }




                                       // FrequencyElementClass FrontMain_030 = FrequencyElementClass.InterpolationByStep(restemp.Main_Polarization.FrequencyElements[o], Step, 0, 30);
                                       // FrequencyElementClass FrontMain_all = FrequencyElementClass.InterpolationByStep(restemp.Main_Polarization.FrequencyElements[o], Step, 330, 360);

                                       //FrequencyElementClass FrontMain_030 = FrequencyElementClass.InterpolationByStep(restemp.Main_Polarization.FrequencyElements[o], Step, 0, 30);
                                       //FrequencyElementClass FrontMain_all = FrequencyElementClass.InterpolationByStep(restemp.Main_Polarization.FrequencyElements[o], Step, 330, 360);
                                       // FrontMain_all.ResultAmpl_PhaseElements.AddRange(FrontMain_030.ResultAmpl_PhaseElements);

                                       CalculationResultsClass.Calculate_DN_Part(FrontMain_all);

                                       #endregion




                                       //расчет кросс поляризации по портам
                                       double CrossPolarMax = double.MinValue;
                                       double CrossPolarMaxAngle = double.NaN;

                                        double MainPolarMax = double.MinValue;
                                        double MainPolarMaxAngle = double.NaN;

                                        if (CrossSpesialResult != null)
                                       {
                                            #region ищем обрабатываемую частоту

                                            for (int z = 0; z < CrossSpesialResult.Cross_Polarization.FrequencyElements.Count; z++)
                                            {
                                                if (CrossSpesialResult.Cross_Polarization.FrequencyElements[z].Frequency == restemp.Main_Polarization.FrequencyElements[o].Frequency)
                                                {
                                                    #region пара найдена, считаем развязку
                                                    FrequencyElementClass tempSpesialCross = FrequencyElementClass.InterpolationByStep(CrossSpesialResult.Cross_Polarization.FrequencyElements[z], Step, Start, Stop);
                                                    
                                                    FrequencyElementClass tempSpesialMainNorm = FrequencyElementClass.InterpolationByStep(CrossSpesialResult.Main_Polarization.FrequencyElements[z], Step, Start, Stop);

                                                    //это вместо клонирования, которое что то не работает
                                                    FrequencyElementClass tempMainNorm = FrequencyElementClass.InterpolationByStep(restemp.Main_Polarization.FrequencyElements[o], Step, Start, Stop);

                                                    //нормируем
                                                    tempSpesialMainNorm= FrequencyElementClass.NormalizationFreqElement(tempSpesialMainNorm);
                                                    tempMainNorm= FrequencyElementClass.NormalizationFreqElement(tempMainNorm);


                                                    List<PointDouble> dataAmpl = CalculationResultsClass.GetListPoint_Ampl_FromFrequencyElement(tempMain);


                                                    PointDouble Max2_Ampl;
                                                    PointDouble Max_LeftAMPL;
                                                    PointDouble Max_RightAMPL;

                                                    int Max_AMPL_index = -1;
                                                    int Max_leftindex = -1;
                                                    int Max_rightindex = -1;
                                                    double WidthDN = 0;


                                                    CalculationClass.CalculationMainMax(dataAmpl, out Max_LeftAMPL, out Max_RightAMPL, out Max2_Ampl, 0, out Max_AMPL_index, out Max_leftindex, out Max_rightindex, out WidthDN);



                                                    for (int b = Max_leftindex; b <= Max_rightindex; b++)
                                                    {
                                                        double tempDeltaCross = tempSpesialCross.ResultAmpl_PhaseElements[b].Ampl_dB - tempMain.ResultAmpl_PhaseElements[b].Ampl_dB;
                                                        double tempDeltaMain = tempSpesialMainNorm.ResultAmpl_PhaseElements[b].Ampl_dB - tempMainNorm.ResultAmpl_PhaseElements[b].Ampl_dB;

                                                        

                                                        if (tempDeltaCross > CrossPolarMax)
                                                        {
                                                            CrossPolarMax = tempDeltaCross;
                                                            CrossPolarMaxAngle = tempMain.ResultAmpl_PhaseElements[b].Cordinate;
                                                        }

                                                        if (tempDeltaMain > MainPolarMax)
                                                        {
                                                            MainPolarMax = tempDeltaMain;
                                                            MainPolarMaxAngle = tempMainNorm.ResultAmpl_PhaseElements[b].Cordinate;
                                                        }
                                                    }


                                                    #endregion
                                                    break;
                                                }
                                            }

                                           #endregion
                                       }


                                       //--------------------

                                       for (int i = 0; i < tempMain.ResultAmpl_PhaseElements.Count; i++)
                                       {
                                           row = new Row() { RowIndex = Convert.ToUInt32(i + 2) };
                                           sheetData.Append(row);


                                           if (tempMain.ResultAmpl_PhaseElements[i].Cordinate != tempCross.ResultAmpl_PhaseElements[i].Cordinate)
                                           {
                                               ResultElementClass remain1 = tempMain.ResultAmpl_PhaseElements[i - 1];
                                               ResultElementClass remain = tempMain.ResultAmpl_PhaseElements[i];
                                               ResultElementClass recros = tempCross.ResultAmpl_PhaseElements[i];
                                           }
                                           else
                                           {
                                               OpenXML_DocumentCreatorClass.InsertCell(row, 1, tempMain.ResultAmpl_PhaseElements[i].Cordinate);
                                               OpenXML_DocumentCreatorClass.InsertCell(row, 1, tempMain.ResultAmpl_PhaseElements[i].Ampl_dB);
                                               OpenXML_DocumentCreatorClass.InsertCell(row, 1, tempCross.ResultAmpl_PhaseElements[i].Ampl_dB);
                                           }


                                           #region добавление рассчитанных данных в соседние столбцы

                                           switch (i)
                                           {
                                               case 0:
                                                   {
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Gain Main, dB");
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, tempMain.CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности);
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, tempMain.CalculationResults.Направление_максимума_диаграммы_направленности);
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");

                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Gain SUM, dB");
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, restemp.PolarizationElements[2].FrequencyElements[o].CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности);
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, restemp.PolarizationElements[2].FrequencyElements[o].CalculationResults.Направление_максимума_диаграммы_направленности);

                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       break;
                                                   }
                                               case 1:
                                                   {
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Beam, deg");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, tempMain.CalculationResults.Ширина_диаграммы_направленности_по_половине_мощности);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       break;
                                                   }
                                               case 2:
                                                   {
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Front-to-back Ratio, dB");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, FrontMain_all.CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности - tempMain.CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, FrontMain_all.CalculationResults.Направление_максимума_диаграммы_направленности);
                                                       // OpenXML_DocumentCreatorClass.InsertCell(row, 1, FrontMain_all.CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       break;
                                                   }
                                               case 3:
                                                    {
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Upper Sidelobe, dB");
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, SpesialUBL);
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, SpesialUBLCoord);

                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, "RightLepestok->");
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, RightLepestok.Y);
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, RightLepestok.X);

                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Grad_178_05->");
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, Find_Grad_178_05.Y);
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, Find_Grad_178_05.X);
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Grad_155_05->");
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, Find_Grad_155_05.Y);
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, Find_Grad_155_05.X);

                                                        // OpenXML_DocumentCreatorClass.InsertCell(row, 1, "LeftLepestok->");


                                                        //для проверки боковых лепестков
                                                        // OpenXML_DocumentCreatorClass.InsertCell(row, 1, LeftLepestok.Y - Max.Y);
                                                        // OpenXML_DocumentCreatorClass.InsertCell(row, 1, LeftLepestok.X);




                                                        break;
                                                    }
                                               case 4:
                                                   {
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Cross-polar Dis, dB");

                                                       if (Double.IsNaN(CrossPolarMaxAngle)) CrossPolarMax = double.NaN;
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, CrossPolarMax);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, CrossPolarMaxAngle);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       break;
                                                   }

                                                case 5:
                                                    {
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Mian-polar Dis, dB");

                                                        if (Double.IsNaN(CrossPolarMaxAngle)) CrossPolarMax = double.NaN;
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, MainPolarMax);
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, MainPolarMaxAngle);
                                                        OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                        break;
                                                    }


                                                default:
                                                   {
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");

                                                       break;
                                                   }
                                           }

                                           #endregion

                                           #region справочные столбцы
                                           /*
                                       if (i < FrontMain_all.ResultAmpl_PhaseElements.Count)
                                       {
                                           OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                           OpenXML_DocumentCreatorClass.InsertCell(row, 1, FrontMain_all.ResultAmpl_PhaseElements[i].Cordinate);
                                           OpenXML_DocumentCreatorClass.InsertCell(row, 1, FrontMain_all.ResultAmpl_PhaseElements[i].Ampl_dB);
                                       }

                                       if (i < Main_Max20.ResultAmpl_PhaseElements.Count)
                                       {
                                           OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                           OpenXML_DocumentCreatorClass.InsertCell(row, 1, Main_Max20.ResultAmpl_PhaseElements[i].Cordinate);
                                           OpenXML_DocumentCreatorClass.InsertCell(row, 1, Main_Max20.ResultAmpl_PhaseElements[i].Ampl_dB);
                                       }
                                       */
                                           #endregion

                                       }
                                   }
                               }
                           }
                       }


                       #endregion


                       workbookPart.Workbook.Save();
                       document.Close();

                       #endregion

                       try
                       {
                           System.Diagnostics.Process.Start(fd.FileName);
                       }
                       catch { }

                       // MessageBox.Show(window, "Экспорт выполнен успешно", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   }
                   catch (Exception ex)
                   {
                       MessageBox.Show(window, string.Format("Ошибка экспорта: \n{0}", ex.Message), "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   }
               }
               else if(Results[0] is IResultType_ДН)
               {
                   try
                   {
                       #region создание xlsx

                       #region Создаем новый документ и настраиваем его
                       SpreadsheetDocument document = SpreadsheetDocument.Create(fd.FileName, SpreadsheetDocumentType.Workbook);

                       WorkbookPart workbookPart = document.AddWorkbookPart();
                       workbookPart.Workbook = new Workbook();

                       FileVersion fv = new FileVersion();
                       fv.ApplicationName = "Microsoft Office Excel";
                       WorkbookStylesPart wbsp = workbookPart.AddNewPart<WorkbookStylesPart>();

                       // Добавляем в документ набор стилей
                       wbsp.Stylesheet = OpenXML_DocumentCreatorClass.GenerateStyleSheet();
                       wbsp.Stylesheet.Save();
                       #endregion

                       #region первый саодный лист в таблице

                       uint[] WidthColumns = { 10, 10, 10, 10, 20, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 };
                       SheetData sheetData = OpenXML_DocumentCreatorClass.CreateSheet(workbookPart, "Сводная таблица", WidthColumns);

                       bool fistAdd = true;



                       for (int j = 0; j < Results.Count; j++)
                       {
                           if (Results[j] is IResultType_ДН)
                           {
                               IResultType_ДН restemp = Results[j] as IResultType_ДН;

                               Row row = new Row();
                               Row row2 = new Row();
                               if (fistAdd)
                               {
                                   row = new Row() { RowIndex = 1 };
                                   sheetData.Append(row);

                                   OpenXML_DocumentCreatorClass.InsertCell(row, 1, "id");
                                   OpenXML_DocumentCreatorClass.InsertCell(row, 2, "port №");
                                   OpenXML_DocumentCreatorClass.InsertCell(row, 3, "Plan");
                                   OpenXML_DocumentCreatorClass.InsertCell(row, 4, "TB deg");
                                   OpenXML_DocumentCreatorClass.InsertCell(row, 5, "Описание");


                                   row2 = new Row() { RowIndex = 2 };
                                   sheetData.Append(row2);
                                   OpenXML_DocumentCreatorClass.InsertCell(row2, 1, "");
                                   OpenXML_DocumentCreatorClass.InsertCell(row2, 1, "");
                                   OpenXML_DocumentCreatorClass.InsertCell(row2, 1, "");
                                   OpenXML_DocumentCreatorClass.InsertCell(row2, 1, "");
                                   OpenXML_DocumentCreatorClass.InsertCell(row2, 1, "");
                               }


                               Row row1 = new Row() { RowIndex = Convert.ToUInt32(j + 4) };
                               sheetData.Append(row1);
                               OpenXML_DocumentCreatorClass.InsertCell(row1, 1, restemp.id);
                               OpenXML_DocumentCreatorClass.InsertCell(row1, 2, "");

                               string adtext = "X3";
                               if (restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут) adtext = "Az";
                               else if (restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан) adtext = "Mer";
                               OpenXML_DocumentCreatorClass.InsertCell(row1, 3, adtext);
                               OpenXML_DocumentCreatorClass.InsertCell(row1, 4, "");
                               OpenXML_DocumentCreatorClass.InsertCell(row1, 5, restemp.MainOptions.Descriptions);

                               for (int i = 0; i < restemp.SelectedPolarization.FrequencyElements.Count; i++)
                               {
                                   if (fistAdd) //заполняем шапку
                                   {
                                       OpenXML_DocumentCreatorClass.InsertCell(row, i * 2 + 6, "Gain, dB");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, i * 2 + 7, "BP, deg");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, i * 2 + 8, "Max, deg");


                                       OpenXML_DocumentCreatorClass.InsertCell(row2, i * 2 + 6, restemp.SelectedPolarization.FrequencyElements[i].Frequency);
                                       OpenXML_DocumentCreatorClass.InsertCell(row2, i * 2 + 7, restemp.SelectedPolarization.FrequencyElements[i].Frequency);
                                       OpenXML_DocumentCreatorClass.InsertCell(row2, i * 2 + 8, restemp.SelectedPolarization.FrequencyElements[i].Frequency);
                                   }

                                   OpenXML_DocumentCreatorClass.InsertCell(row1, i * 2 + 6, restemp.SelectedPolarization.FrequencyElements[i].CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности);
                                   OpenXML_DocumentCreatorClass.InsertCell(row1, i * 2 + 7, restemp.SelectedPolarization.FrequencyElements[i].CalculationResults.Ширина_диаграммы_направленности_по_половине_мощности);
                                   OpenXML_DocumentCreatorClass.InsertCell(row1, i * 2 + 8, restemp.SelectedPolarization.FrequencyElements[i].CalculationResults.Направление_максимума_диаграммы_направленности);
                               }


                               fistAdd = false;
                           }


                       }

                       #endregion

                       #region второй лист неоднородности

                       sheetData = OpenXML_DocumentCreatorClass.CreateSheet(workbookPart, "Неоднородность", WidthColumns);

                       Row rowN = new Row() { RowIndex = 1 };
                       sheetData.Append(rowN);
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Частота, МГц");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Неоднородность, дБ");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Координата");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Порт, TB № имерения минимального");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Значение минимального");

                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Порт, TB № имерения макс");
                       OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "Значение макс");


                       List<List<FrequencyElementDescriptionClass>> Azimut = new List<List<FrequencyElementDescriptionClass>>();
                       List<List<FrequencyElementDescriptionClass>> Meridian = new List<List<FrequencyElementDescriptionClass>>();

                       List<List<FrequencyElementDescriptionClass>> Azimut2 = new List<List<FrequencyElementDescriptionClass>>();


                       #region разбиваем по рабочим портам и частотам

                       for (int j = 0; j < Results.Count; j++)
                       {
                           if (Results[j] is IResultType_ДН)
                           {
                               IResultType_ДН restemp = Results[j] as IResultType_ДН;

                               int portN = -1;
                               int tbN = -1;

                               int startF = 0;
                               int stopF = restemp.SelectedPolarization.FrequencyElements.Count;

                               SaverResultForm.DecodingDescription(restemp, out portN, out tbN, out startF, out stopF);


                               for (int s = startF; s <= stopF; s++)
                               {
                                   //делаем интерполяцию для последущего сложения результатов
                                   double Step;
                                   double Start;
                                   double Stop;
                                   ResultType_MAINClass.GetCoordinatForInterpolation(restemp.MainOptions, out Start, out Stop, out Step);
                                   Step = 0.5;
                                   FrequencyElementClass tempMain = FrequencyElementClass.InterpolationByStep(restemp.SelectedPolarization.FrequencyElements[s], Step, Start, Stop);


                                   FrequencyElementClass freq = tempMain;

                                   //для азимутальных измерений
                                   if (restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Азимут || restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут)
                                   {
                                       bool Find = false;
                                       for (int z = 0; z < Azimut.Count; z++)
                                       {
                                           List<FrequencyElementDescriptionClass> ListFreq = Azimut[z];

                                           if (ListFreq[0].Freq.Frequency == freq.Frequency)
                                           {
                                               ListFreq.Add(new FrequencyElementDescriptionClass(portN, tbN, restemp.id, freq));

                                               //дублируем ещё в один массив
                                               Azimut2[z].Add(new FrequencyElementDescriptionClass(portN, tbN, restemp.id, freq));

                                               Find = true;
                                               break;
                                           }
                                       }

                                       if (!Find)
                                       {
                                           //если не нашли в массиве, то добавляем новый
                                           List<FrequencyElementDescriptionClass> TempFreq = new List<FrequencyElementDescriptionClass>();
                                           TempFreq.Add(new FrequencyElementDescriptionClass(portN, tbN, restemp.id, freq));
                                           Azimut.Add(TempFreq);

                                           //дублируем ещё в один массив
                                           List<FrequencyElementDescriptionClass> TempFreq2 = new List<FrequencyElementDescriptionClass>();
                                           TempFreq2.Add(new FrequencyElementDescriptionClass(portN, tbN, restemp.id, freq));
                                           Azimut2.Add(TempFreq2);
                                       }
                                   }

                                       //для меридианальных измерений
                                   else if (restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.Суммарная_ДН_Меридиан || restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан)
                                   {
                                       bool Find = false;
                                       foreach (List<FrequencyElementDescriptionClass> ListFreq in Meridian)
                                       {
                                           if (ListFreq[0].Freq.Frequency == freq.Frequency)
                                           {
                                               ListFreq.Add(new FrequencyElementDescriptionClass(portN, tbN, restemp.id, freq));
                                               Find = true;
                                           }
                                       }

                                       if (!Find)
                                       {
                                           //если не нашли в массиве, то добавляем новый
                                           List<FrequencyElementDescriptionClass> TempFreq = new List<FrequencyElementDescriptionClass>();
                                           TempFreq.Add(new FrequencyElementDescriptionClass(portN, tbN, restemp.id, freq));
                                           Meridian.Add(TempFreq);
                                       }
                                   }

                               }
                           }
                       }

                       #endregion




                       int rowCount = 0;

                       #region По меридиану
                       for (int z = 0; z < Meridian.Count; z++)
                       {
                           //выбрали массив на одной частоте
                           List<FrequencyElementDescriptionClass> TempFreqMass = Meridian[z];

                           double LeftMax = double.MaxValue;
                           double RightMax = double.MinValue;

                           foreach (FrequencyElementDescriptionClass TempFreq in TempFreqMass)
                           {
                               //перебираем данные частот на разных измерениях
                               List<PointDouble> dataAmplMain = CalculationResultsClass.GetListPoint_Ampl_FromFrequencyElement(TempFreq.Freq);

                               PointDouble Max;
                               PointDouble Left05;
                               PointDouble Right05;
                               PointDouble Max2;
                               int MaxIndex;
                               int LeftIndex;
                               int RightIndex;
                               double winghDN;

                               PointDouble LeftLepestok;
                               PointDouble RightLepestok;

                               Max = CalculationClass.CalculationMainMax(dataAmplMain, out Left05, out Right05, out Max2, 1, out MaxIndex, out LeftIndex, out RightIndex, out winghDN, out LeftLepestok, out RightLepestok, 0.2d);


                               if (Left05.X < LeftMax) LeftMax = Left05.X;
                               if (Right05.X > RightMax) RightMax = Right05.X;

                           }



                           //прошли все частоты, теперь режем диаграммы по максимальными растояниям от максимума ДН

                           foreach (FrequencyElementDescriptionClass TempFreq in TempFreqMass)
                           {
                               FrequencyElementClass tempMain = FrequencyElementClass.InterpolationByStep(TempFreq.Freq, 0.5d, LeftMax, RightMax);
                               //нормализацию сюда
                               TempFreq.Freq = tempMain;
                           }

                           double MaxDistange = double.MinValue;
                           double MaxTop = double.MinValue;
                           double MaxBottom = double.MaxValue;
                           string DiscriptionTop = "";
                           string DiscriptionBottom = "";
                           string Andgle = "";

                           for (int i = 0; i < TempFreqMass[0].Freq.ResultAmpl_PhaseElements.Count; i++)
                           {
                               double MaxTopTemp = double.MinValue;
                               double MaxBottomTemp = double.MaxValue;
                               string DiscriptionTopTemp = "";
                               string DiscriptionBottomTemp = "";


                               for (int j = 0; j < TempFreqMass.Count; j++)
                               {
                                   if (MaxTopTemp < TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxTopTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionTopTemp = TempFreqMass[j].ToString();
                                   }
                                   if (MaxBottomTemp > TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxBottomTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionBottomTemp = TempFreqMass[j].ToString();
                                   }
                               }

                               if (MaxTopTemp - MaxBottomTemp > MaxDistange)
                               {
                                   MaxDistange = MaxTopTemp - MaxBottomTemp;
                                   MaxTop = MaxTopTemp;
                                   MaxBottom = MaxBottomTemp;

                                   Andgle = TempFreqMass[0].Freq.ResultAmpl_PhaseElements[i].Cordinate.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                                   DiscriptionBottom = DiscriptionBottomTemp;
                                   DiscriptionTop = DiscriptionTopTemp;
                               }
                           }

                           //записываем результат в эксельку

                           rowN = new Row() { RowIndex = Convert.ToUInt32(rowCount + 2) };
                           rowCount++;
                           sheetData.Append(rowN);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "MerV1");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, TempFreqMass[0].Freq.Frequency);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxDistange);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, Andgle);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionTop);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxTop);
                       }
                       #endregion



                       #region По азимуту
                       for (int z = 0; z < Azimut.Count; z++)
                       {
                           //выбрали массив на одной частоте
                           List<FrequencyElementDescriptionClass> TempFreqMass = Azimut[z];

                           double LeftMax = double.MaxValue;
                           double RightMax = double.MinValue;

                           foreach (FrequencyElementDescriptionClass TempFreq in TempFreqMass)
                           {
                               //перебираем данные частот на разных измерениях




                               List<PointDouble> dataAmplMain = CalculationResultsClass.GetListPoint_Ampl_FromFrequencyElement(TempFreq.Freq);

                               PointDouble Max;
                               PointDouble Left05;
                               PointDouble Right05;
                               PointDouble Max2;
                               int MaxIndex;
                               int LeftIndex;
                               int RightIndex;
                               double winghDN;

                               PointDouble LeftLepestok;
                               PointDouble RightLepestok;

                               Max = CalculationClass.CalculationMainMax(dataAmplMain, out Left05, out Right05, out Max2, 1, out MaxIndex, out LeftIndex, out RightIndex, out winghDN, out LeftLepestok, out RightLepestok, 0.2d);


                               if (Left05.X < LeftMax) LeftMax = Left05.X;
                               if (Right05.X > RightMax) RightMax = Right05.X;

                           }

                           //прошли все частоты, теперь режем диаграммы по максимальными растояниям от максимума ДН

                           foreach (FrequencyElementDescriptionClass TempFreq in TempFreqMass)
                           {
                               FrequencyElementClass tempMain = FrequencyElementClass.InterpolationByStep(TempFreq.Freq, 0.5d, LeftMax, RightMax);
                               TempFreq.Freq = tempMain;
                           }

                           double MaxDistange = double.MinValue;
                           double MaxTop = double.MinValue;
                           double MaxBottom = double.MaxValue;
                           string DiscriptionTop = "";
                           string DiscriptionBottom = "";
                           string Andgle = "";

                           for (int i = 0; i < TempFreqMass[0].Freq.ResultAmpl_PhaseElements.Count; i++)
                           {
                               double MaxTopTemp = double.MinValue;
                               double MaxBottomTemp = double.MaxValue;
                               string DiscriptionTopTemp = "";
                               string DiscriptionBottomTemp = "";


                               for (int j = 0; j < TempFreqMass.Count; j++)
                               {
                                   if (MaxTopTemp < TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxTopTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionTopTemp = TempFreqMass[j].ToString();
                                   }
                                   if (MaxBottomTemp > TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxBottomTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionBottomTemp = TempFreqMass[j].ToString();
                                   }
                               }

                               if (MaxTopTemp - MaxBottomTemp > MaxDistange)
                               {
                                   MaxDistange = MaxTopTemp - MaxBottomTemp;
                                   MaxTop = MaxTopTemp;
                                   MaxBottom = MaxBottomTemp;

                                   Andgle = TempFreqMass[0].Freq.ResultAmpl_PhaseElements[i].Cordinate.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                                   DiscriptionBottom = DiscriptionBottomTemp;
                                   DiscriptionTop = DiscriptionTopTemp;
                               }
                           }

                           //записываем результат в эксельку

                           rowN = new Row() { RowIndex = Convert.ToUInt32(rowCount + 3) };
                           rowCount++;
                           sheetData.Append(rowN);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "AzV1");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, TempFreqMass[0].Freq.Frequency);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxDistange);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, Andgle);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionTop);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxTop);
                       }
                       #endregion

                       #region По азимуту 2вар
                       for (int z = 0; z < Azimut2.Count; z++)
                       {
                           //выбрали массив на одной частоте
                           List<FrequencyElementDescriptionClass> TempFreqMass = Azimut2[z];

                           double LeftMax = double.MaxValue;
                           double RightMax = double.MinValue;

                           //диапазоны не совсем верно ищутся, при необходимости поправить

                           if (TempFreqMass[0].Freq.Frequency <= 850 || TempFreqMass[0].Freq.Frequency < 1850 && TempFreqMass[0].Freq.Frequency >= 1700)
                           {
                               LeftMax = 180d - 67d / 2d;
                               RightMax = 180d + 67d / 2d;
                           }
                           else if (TempFreqMass[0].Freq.Frequency >= 850 && TempFreqMass[0].Freq.Frequency < 950 || TempFreqMass[0].Freq.Frequency >= 2300)
                           {
                               LeftMax = 180d - 64d / 2d;
                               RightMax = 180d + 64d / 2d;
                           }
                           else if (TempFreqMass[0].Freq.Frequency >= 1900 && TempFreqMass[0].Freq.Frequency < 2200)
                           {
                               LeftMax = 180d - 65d / 2d;
                               RightMax = 180d + 65d / 2d;
                           }

                           //прошли все частоты, теперь режем диаграммы по максимальными растояниям от максимума ДН

                           foreach (FrequencyElementDescriptionClass TempFreq in TempFreqMass)
                           {
                               FrequencyElementClass tempMain = FrequencyElementClass.InterpolationByStep(TempFreq.Freq, 0.5d, LeftMax, RightMax);
                               TempFreq.Freq = tempMain;

                               if (TempFreq.Freq.Frequency == 1927.4d && TempFreq.Port==3) 
                               {//пауза
                               }
                           }

                           double MaxDistange = double.MinValue;
                           double MaxTop = double.MinValue;
                           double MaxBottom = double.MaxValue;
                           string DiscriptionTop = "";
                           string DiscriptionBottom = "";
                           string Andgle = "";

                           for (int i = 0; i < TempFreqMass[0].Freq.ResultAmpl_PhaseElements.Count; i++)
                           {
                               double MaxTopTemp = double.MinValue;
                               double MaxBottomTemp = double.MaxValue;
                               string DiscriptionTopTemp = "";
                               string DiscriptionBottomTemp = "";


                               for (int j = 0; j < TempFreqMass.Count; j++)
                               {
                                   if (MaxTopTemp < TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxTopTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionTopTemp = TempFreqMass[j].ToString();
                                   }
                                   if (MaxBottomTemp > TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxBottomTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionBottomTemp = TempFreqMass[j].ToString();
                                   }
                               }

                               if (MaxTopTemp - MaxBottomTemp > MaxDistange)
                               {
                                   MaxDistange = MaxTopTemp - MaxBottomTemp;
                                   MaxTop = MaxTopTemp;
                                   MaxBottom = MaxBottomTemp;

                                   Andgle = TempFreqMass[0].Freq.ResultAmpl_PhaseElements[i].Cordinate.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                                   DiscriptionBottom = DiscriptionBottomTemp;
                                   DiscriptionTop = DiscriptionTopTemp;
                               }
                           }

                           //записываем результат в эксельку

                           rowN = new Row() { RowIndex = Convert.ToUInt32(rowCount + 4) };
                           rowCount++;
                           sheetData.Append(rowN);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "AzV2");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, TempFreqMass[0].Freq.Frequency);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxDistange);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, Andgle);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionTop);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxTop);
                       }
                       #endregion

                       #region Разбивка по связанным портам

                       List<List<FrequencyElementDescriptionClass>> Azimut12 = new List<List<FrequencyElementDescriptionClass>>();
                       List<List<FrequencyElementDescriptionClass>> Azimut34 = new List<List<FrequencyElementDescriptionClass>>();
                       List<List<FrequencyElementDescriptionClass>> Azimut56 = new List<List<FrequencyElementDescriptionClass>>();
                       List<List<FrequencyElementDescriptionClass>> Azimut78 = new List<List<FrequencyElementDescriptionClass>>();

                       //используем уже порезанные массивы
                       for (int z = 0; z < Azimut2.Count; z++)
                       {
                           //выбрали массив на одной частоте
                           List<FrequencyElementDescriptionClass> TempFreqMass = Azimut2[z];

                           for (int i = 0; i < TempFreqMass.Count; i++)
                           {
                               switch (TempFreqMass[i].Port)
                               {
                                   case 1:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut12, TempFreqMass[i]);
                                           break;
                                       }
                                   case 2:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut12, TempFreqMass[i]);
                                           break;
                                       }
                                   case 3:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut34, TempFreqMass[i]);
                                           break;
                                       }
                                   case 4:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut34, TempFreqMass[i]);
                                           break;
                                       }
                                   case 5:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut56, TempFreqMass[i]);
                                           break;
                                       }
                                   case 6:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut56, TempFreqMass[i]);
                                           break;
                                       }
                                   case 7:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut78, TempFreqMass[i]);
                                           break;
                                       }
                                   case 8:
                                       {
                                           AddToFrequencyElementDescriptionList(ref Azimut78, TempFreqMass[i]);
                                           break;
                                       }
                                   default:
                                       {

                                           break;
                                       }
                               }
                           }
                       }



                       #endregion

                       #region расчет по связанным портам
                       //скидываем все в один массив
                       List<List<FrequencyElementDescriptionClass>> AzimutPortFull = new List<List<FrequencyElementDescriptionClass>>();
                       AzimutPortFull.AddRange(Azimut12);
                       AzimutPortFull.AddRange(Azimut34);
                       AzimutPortFull.AddRange(Azimut56);
                       AzimutPortFull.AddRange(Azimut78);


                       for (int z = 0; z < AzimutPortFull.Count; z++)
                       {
                           //выбрали массив на одной частоте
                           List<FrequencyElementDescriptionClass> TempFreqMass = AzimutPortFull[z];


                           double MaxDistange = double.MinValue;
                           double MaxTop = double.MinValue;
                           double MaxBottom = double.MaxValue;
                           string DiscriptionTop = "";
                           string DiscriptionBottom = "";
                           string Andgle = "";

                           for (int i = 0; i < TempFreqMass[0].Freq.ResultAmpl_PhaseElements.Count; i++)
                           {
                               double MaxTopTemp = double.MinValue;
                               double MaxBottomTemp = double.MaxValue;
                               string DiscriptionTopTemp = "";
                               string DiscriptionBottomTemp = "";


                               for (int j = 0; j < TempFreqMass.Count; j++)
                               {
                                   if (MaxTopTemp < TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxTopTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionTopTemp = TempFreqMass[j].ToString();
                                   }
                                   if (MaxBottomTemp > TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB)
                                   {
                                       MaxBottomTemp = TempFreqMass[j].Freq.ResultAmpl_PhaseElements[i].Ampl_dB;
                                       DiscriptionBottomTemp = TempFreqMass[j].ToString();
                                   }
                               }

                               if (MaxTopTemp - MaxBottomTemp > MaxDistange)
                               {
                                   MaxDistange = MaxTopTemp - MaxBottomTemp;
                                   MaxTop = MaxTopTemp;
                                   MaxBottom = MaxBottomTemp;

                                   Andgle = TempFreqMass[0].Freq.ResultAmpl_PhaseElements[i].Cordinate.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                                   DiscriptionBottom = DiscriptionBottomTemp;
                                   DiscriptionTop = DiscriptionTopTemp;
                               }
                           }

                           //записываем результат в эксельку

                           rowN = new Row() { RowIndex = Convert.ToUInt32(rowCount + 5) };
                           rowCount++;
                           sheetData.Append(rowN);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "AzV3");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, TempFreqMass[0].Freq.Frequency);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxDistange);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, "");
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, Andgle);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxBottom);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, DiscriptionTop);
                           OpenXML_DocumentCreatorClass.InsertCell(rowN, 1, MaxTop);
                       }



                       #endregion

                       #endregion


                       #region добавляем значения измерений и частот

                       for (int j = 0; j < Results.Count; j++)
                       {
                           if (Results[j] is IResultType_ДН)
                           {
                               IResultType_ДН restemp = Results[j] as IResultType_ДН;

                               IResultType_ДН CrossSpesialResult = null;

                               int portN = -1;
                               int tbN = -1;

                               int startF = 0;
                               int stopF = restemp.SelectedPolarization.FrequencyElements.Count;

                               SaverResultForm.DecodingDescription(restemp, out portN, out tbN, out startF, out stopF);
                               
                             

                               if (stopF <= restemp.SelectedPolarization.FrequencyElements.Count)
                               {
                                   for (int o = startF; o <= stopF; o++)
                                   {
                                       string adtext = "X3HV";
                                       if (restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Азимут) adtext = "H";
                                       else if (restemp.MainOptions.MeasurementResultType == MeasurementTypeEnum.ДН_Меридиан) adtext = "V";


                                       string sheetName = string.Format("Port{0} TB{1} {2}{3}", portN.ToString(), tbN.ToString(), adtext, restemp.SelectedPolarization.FrequencyElements[o].Frequency);
                                       if (sheetName == "Port3 TB7 V1838,8")
                                       { //пауза
                                       }

                                       sheetData = OpenXML_DocumentCreatorClass.CreateSheet(workbookPart, sheetName, WidthColumns);

                                       Row row = new Row() { RowIndex = 1 };
                                       sheetData.Append(row);

                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Angle");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Main");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Cross");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Parameter");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Volume");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Position");
                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, restemp.id);

                                       double Step;
                                       double Start;
                                       double Stop;

                                       ResultType_MAINClass.GetCoordinatForInterpolation(restemp.MainOptions, out Start, out Stop, out Step,true);

                                       Step = 0.5;

                                       FrequencyElementClass tempMain = FrequencyElementClass.InterpolationByStep(restemp.SelectedPolarization.FrequencyElements[o], Step, Start, Stop);

                                       CalculationResultsClass.Calculate_DN_Part(tempMain);
                                      


                                       //----------------

                                       #region рассчет бокового лепестка влево
                                       //искать будем по мэйну
                                       List<PointDouble> dataAmplMain = CalculationResultsClass.GetListPoint_Ampl_FromFrequencyElement(tempMain);

                                       PointDouble Max;
                                       PointDouble Left05;
                                       PointDouble Right05;
                                       PointDouble Max2;
                                       int MaxIndex;
                                       int LeftIndex;
                                       int RightIndex;
                                       double winghDN;

                                       PointDouble LeftLepestok;
                                       PointDouble RightLepestok;

                                       Max = CalculationClass.CalculationMainMax(dataAmplMain, out Left05, out Right05, out Max2, 1, out MaxIndex, out LeftIndex, out RightIndex, out winghDN, out LeftLepestok, out RightLepestok, 0.2d);

                                       double Grad_178_05 = winghDN * 1.78d;
                                       double SpesialUBL = double.NaN;
                                       double SpesialUBLCoord = double.NaN;
                                       double SpesialUBL20 = double.NaN;
                                       double SpesialUBLCoord20 = double.NaN;


                                       #region проверка на выход за границы 1.78 ширины ДН
                                       if (Max.X - Grad_178_05 <= LeftLepestok.X)
                                       {
                                           // в границах все ок
                                           SpesialUBL = LeftLepestok.Y - Max.Y;
                                           SpesialUBLCoord = LeftLepestok.X;
                                       }
                                       else
                                       {
                                           double Grad_155_05 = Max.X - winghDN * 1.55d;

                                           //ищем точки рядом с найденой координатой

                                           for (int i = 0; i < dataAmplMain.Count - 1; i++)
                                           {
                                               if (dataAmplMain[i].X <= Grad_155_05 && dataAmplMain[i + 1].X >= Grad_155_05)
                                               {
                                                   if (Math.Abs(dataAmplMain[i].X - Grad_155_05) <= Math.Abs(dataAmplMain[i + 1].X - Grad_155_05))
                                                   {
                                                       SpesialUBL = dataAmplMain[i].Y - Max.Y;
                                                       SpesialUBLCoord = dataAmplMain[i].X;
                                                   }
                                                   else
                                                   {
                                                       SpesialUBL = dataAmplMain[i + 1].Y - Max.Y;
                                                       SpesialUBLCoord = dataAmplMain[i + 1].X;
                                                   }
                                               }
                                           }
                                       }
                                       #endregion

                                       #region проверка на выход за границы -20град ширины ДН
                                       if (Max.X - 20 <= LeftLepestok.X)
                                       {
                                           // в границах все ок
                                           SpesialUBL20 = LeftLepestok.Y - Max.Y;
                                           SpesialUBLCoord20 = LeftLepestok.X;
                                       }
                                       else
                                       {
                                           //ищем точки рядом с найденой координатой

                                           for (int i = 0; i < dataAmplMain.Count - 1; i++)
                                           {
                                               if (dataAmplMain[i].X <= (Max.X - 20) && dataAmplMain[i + 1].X >= (Max.X - 20))
                                               {
                                                   if (Math.Abs(dataAmplMain[i].X - (Max.X - 20)) <= Math.Abs(dataAmplMain[i + 1].X - (Max.X - 20)))
                                                   {
                                                       SpesialUBL20 = dataAmplMain[i].Y - Max.Y;
                                                       SpesialUBLCoord20 = dataAmplMain[i].X;
                                                   }
                                                   else
                                                   {
                                                       SpesialUBL20 = dataAmplMain[i + 1].Y - Max.Y;
                                                       SpesialUBLCoord20 = dataAmplMain[i + 1].X;
                                                   }
                                               }
                                           }
                                       }
                                       #endregion


                                       #endregion


                                       //-----------------


                                       #region расчет front to back Ratio

                                       List<PointDouble> LeftAmplMain = null;
                                       List<PointDouble> RightAmplMain = null;

                                       CalculationClass.DeleteMassiv_By_Max(dataAmplMain, MaxIndex, out LeftAmplMain, out RightAmplMain);



                                       FrequencyElementClass FrontMain_all = new FrequencyElementClass();

                                       foreach (PointDouble point in RightAmplMain)
                                       {
                                           if (point.X <= RightAmplMain[RightAmplMain.Count - 1].X && point.X >= RightAmplMain[RightAmplMain.Count - 1].X - 30)
                                           {
                                               FrontMain_all.ResultAmpl_PhaseElements.Add(new ResultElementClass(point.X, point.Y, 0));
                                           }
                                       }


                                       foreach (PointDouble point in LeftAmplMain)
                                       {
                                           if (point.X <= LeftAmplMain[0].X + 30 && point.X >= LeftAmplMain[0].X)
                                           {
                                               FrontMain_all.ResultAmpl_PhaseElements.Add(new ResultElementClass(point.X, point.Y, 0));
                                           }
                                       }


                                       CalculationResultsClass.Calculate_DN_Part(FrontMain_all);

                                       #endregion


                                       //--------------------

                                       for (int i = 0; i < tempMain.ResultAmpl_PhaseElements.Count; i++)
                                       {
                                           row = new Row() { RowIndex = Convert.ToUInt32(i + 2) };
                                           sheetData.Append(row);


                                          
                                               OpenXML_DocumentCreatorClass.InsertCell(row, 1, tempMain.ResultAmpl_PhaseElements[i].Cordinate);
                                               OpenXML_DocumentCreatorClass.InsertCell(row, 1, tempMain.ResultAmpl_PhaseElements[i].Ampl_dB);
                                               OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");


                                           #region добавление рассчитанных данных в соседние столбцы

                                           switch (i)
                                           {
                                               case 0:
                                                   {
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Gain, dB");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, tempMain.CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, tempMain.CalculationResults.Направление_максимума_диаграммы_направленности);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       break;
                                                   }
                                               case 1:
                                                   {
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Beam, deg");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, tempMain.CalculationResults.Ширина_диаграммы_направленности_по_половине_мощности);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       break;
                                                   }
                                               case 2:
                                                   {
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Front-to-back Ratio, dB");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, FrontMain_all.CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности - tempMain.CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, FrontMain_all.CalculationResults.Направление_максимума_диаграммы_направленности);
                                                       // OpenXML_DocumentCreatorClass.InsertCell(row, 1, FrontMain_all.CalculationResults.Коэффициент_усиления_в_максимуме_диаграммы_направленности);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       break;
                                                   }
                                               case 3:
                                                   {
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Upper Sidelobe, dB");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, SpesialUBL);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, SpesialUBLCoord);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "-20 град->");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, SpesialUBL20);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, SpesialUBLCoord20);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "LeftLepestok->");


                                                       //для проверки боковых лепестков
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, LeftLepestok.Y - Max.Y);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, LeftLepestok.X);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "RightLepestok->");

                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, RightLepestok.Y - Max.Y);
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, RightLepestok.X);

                                                       break;
                                                   }
                                               case 4:
                                                   {
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "Cross-polar Dis, dB");

                                                       break;
                                                   }


                                               default:
                                                   {
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                                       OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");

                                                       break;
                                                   }
                                           }

                                           #endregion

                                           #region справочные столбцы
                                           /*
                                       if (i < FrontMain_all.ResultAmpl_PhaseElements.Count)
                                       {
                                           OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                           OpenXML_DocumentCreatorClass.InsertCell(row, 1, FrontMain_all.ResultAmpl_PhaseElements[i].Cordinate);
                                           OpenXML_DocumentCreatorClass.InsertCell(row, 1, FrontMain_all.ResultAmpl_PhaseElements[i].Ampl_dB);
                                       }

                                       if (i < Main_Max20.ResultAmpl_PhaseElements.Count)
                                       {
                                           OpenXML_DocumentCreatorClass.InsertCell(row, 1, "");
                                           OpenXML_DocumentCreatorClass.InsertCell(row, 1, Main_Max20.ResultAmpl_PhaseElements[i].Cordinate);
                                           OpenXML_DocumentCreatorClass.InsertCell(row, 1, Main_Max20.ResultAmpl_PhaseElements[i].Ampl_dB);
                                       }
                                       */
                                           #endregion

                                       }
                                   }
                               }
                           }
                       }


                       #endregion


                       workbookPart.Workbook.Save();
                       document.Close();

                       #endregion

                       try
                       {
                           System.Diagnostics.Process.Start(fd.FileName);
                       }
                       catch { }

                       // MessageBox.Show(window, "Экспорт выполнен успешно", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   }
                   catch (Exception ex)
                   {
                       MessageBox.Show(window, string.Format("Ошибка экспорта: \n{0}", ex.Message), "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   }
               }
           }
       }


        #region вспомогательные функции для XLS документов

       public static void DecodingDescription(IResultType_MAIN restemp, out int portN, out int tbN, out int startF, out int stopF)
       {
           string port="";
           string tb="";
           portN = -1; tbN = -1; startF = -1; stopF = -1;

           if (restemp.MainOptions.Descriptions != "")
           {
               List<string> dis2 = new List<string>(restemp.MainOptions.Descriptions.Split('%'));

               dis2[0] = dis2[0].Trim(' ');
               dis2[1] = dis2[1].Trim(' ');

                List<string> port_TB = new List<string>(dis2[0].Split('t'));

                 port = port_TB[0];
                 tb = port_TB[1];

               portN = Convert.ToInt32(port);
               tbN = Convert.ToInt32(tb);

               List<string> disF = new List<string>(dis2[1].Split('f'));

               startF = Convert.ToInt32(disF[0]);
               stopF = Convert.ToInt32(disF[1]);
           }
       }

       protected class FrequencyElementDescriptionClass
       {
           public FrequencyElementDescriptionClass(int PortN, int TBN, int idn, FrequencyElementClass freq)
           {
               Port = PortN;
               TB = TBN;
               ID = idn;
               Freq = freq;
           }


           public int Port = -1;
           public int TB = -1;
           public int ID = -1;

           public FrequencyElementClass Freq = null;

           public override string ToString()
           {
               return string.Format("{0} Port{1} TB{2} id{3}", Freq, Port, TB, ID);
           }
       }

       protected static void AddToFrequencyElementDescriptionList(ref List<List<FrequencyElementDescriptionClass>> ToList, FrequencyElementDescriptionClass AddObj)
       {
          bool Find = false;

           for (int z = 0; z < ToList.Count; z++)
           {
               List<FrequencyElementDescriptionClass> ListFreq = ToList[z];

               if (ListFreq[0].Freq.Frequency == AddObj.Freq.Frequency)
               {
                   ListFreq.Add(AddObj);
                   Find = true;
                   break;
               }
           }

           if (!Find)
           {
               //если не нашли в массиве, то добавляем новый
               List<FrequencyElementDescriptionClass> TempFreq = new List<FrequencyElementDescriptionClass>();
               TempFreq.Add(AddObj);
               ToList.Add(TempFreq);
           }

       }

        protected static PointDouble FindPointNear(List<PointDouble> dataAmplMain, double FindCoordinat)
        {
            PointDouble ret = new PointDouble();

            for (int i = 0; i < dataAmplMain.Count - 1; i++)
            {
                if (dataAmplMain[i].X <= FindCoordinat && dataAmplMain[i + 1].X >= FindCoordinat)
                {
                    if (Math.Abs(dataAmplMain[i].X - FindCoordinat) <= Math.Abs(dataAmplMain[i + 1].X - FindCoordinat))
                    {
                        ret.Y = dataAmplMain[i].Y;
                        ret.X = dataAmplMain[i].X;
                    }
                    else
                    {
                        ret.Y = dataAmplMain[i + 1].Y;
                        ret.X = dataAmplMain[i + 1].X;
                    }
                }
            }

            return ret;
        }


     

            #endregion

            #endregion


        }
}
