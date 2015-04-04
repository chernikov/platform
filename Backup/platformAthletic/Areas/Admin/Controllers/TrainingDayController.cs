using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using platformAthletic.Model;
using platformAthletic.Models.Info;
using platformAthletic.Models.ViewModels;

namespace platformAthletic.Areas.Admin.Controllers
{
    public class TrainingDayController : AdminController
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private static string SessionFilterName = "TrainingDayFilter";

        public ActionResult Index()
        {
            FilterTrainingDayInfo filterTrainingDayInfo = null;
            if (Session[SessionFilterName] == null)
            {
                filterTrainingDayInfo = new FilterTrainingDayInfo()
                {
                    WeekID = null,
                    PhaseID = null
                };
                Session[SessionFilterName] = filterTrainingDayInfo;
            }
            else
            {
                filterTrainingDayInfo = Session[SessionFilterName] as FilterTrainingDayInfo;
            }
            var list = Repository.TrainingDays;
            
            if (filterTrainingDayInfo.WeekID.HasValue)
            {
                list = list.Where(p => p.WeekID == filterTrainingDayInfo.WeekID);
            }
            if (filterTrainingDayInfo.PhaseID.HasValue)
            {
                list = list.Where(p => p.Week.PhaseID == filterTrainingDayInfo.PhaseID);
            }
            ViewBag.Search = filterTrainingDayInfo;

            return View(list.OrderBy(p => p.WeekID).ThenBy(p => p.DayID).ToList());
        }

        [HttpPost]
        public ActionResult Index(FilterTrainingDayInfo filterTrainintDayInfo)
        {
            Session[SessionFilterName] = filterTrainintDayInfo;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            var trainingDayView = new TrainingDayView();
            return View("Edit", trainingDayView);
        }

        public ActionResult GenerateInSeason()
        {
            foreach (var week in Repository.Weeks.Where(p => p.Phase.Cycle.SeasonID == 2).ToList())
            {
                foreach (var day in Repository.Days.ToList())
                {
                    var trainingDay = new TrainingDay()
                    {
                        DayID = day.ID,
                        WeekID = week.ID
                    };
                    Repository.CreateTrainingDay(trainingDay);
                }
            }
            return null;
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var trainingDay = Repository.TrainingDays.FirstOrDefault(p => p.ID == id);
            if (trainingDay != null)
            {
                var trainingDayView = (TrainingDayView)ModelMapper.Map(trainingDay, typeof(TrainingDay), typeof(TrainingDayView));
                return View(trainingDayView);
            }
            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult Edit(TrainingDayView trainingDayView)
        {
            if (ModelState.IsValid)
            {
                var trainingDay = (TrainingDay)ModelMapper.Map(trainingDayView, typeof(TrainingDayView), typeof(TrainingDay));
                if (trainingDay.ID == 0)
                {
                    Repository.CreateTrainingDay(trainingDay);
                }
                else
                {
                    Repository.UpdateTrainingDay(trainingDay);
                }
                return RedirectToAction("Index");
            }
            return View(trainingDayView);
        }

        [HttpGet]
        public ActionResult EditTable(int id)
        {
            var trainingDay = Repository.TrainingDays.FirstOrDefault(p => p.ID == id);
            if (trainingDay != null)
            {
                var trainingDayView = (TrainingDayView)ModelMapper.Map(trainingDay, typeof(TrainingDay), typeof(TrainingDayView));
                return View(trainingDayView);
            }
            return RedirectToNotFoundPage;
        }

        public ActionResult UpdateCells()
        {
            var i = 0;
            var type = 0;
            foreach (var cell in Repository.Cells)
            {
                if (type != cell.Type)
                {
                    type = cell.Type;
                    i = 1;
                }
                cell.Name = string.Format("{0} {1}", ((Cell.CellType)cell.Type).ToString(), i);
                Repository.UpdateCell(cell);
                i++;
            }
            return null;
        }

        [HttpGet]
        public ActionResult EditCellInfo(int id, int trainingDayID)
        {
            var trainingDayCell = Repository.TrainingDayCells.FirstOrDefault(p => p.CellID == id && p.TrainingDayID == trainingDayID);
            if (trainingDayCell != null)
            {
                var trainingDayCellView = (TrainingDayCellView)ModelMapper.Map(trainingDayCell, typeof(TrainingDayCell), typeof(TrainingDayCellView));
                return View(trainingDayCellView);
            }
            else
            {
                var trainingDayCellView = new TrainingDayCellView()
                {
                    CellID = id,
                    TrainingDayID = trainingDayID
                };
                return View(trainingDayCellView);
            }
        }

        [HttpPost]
        public ActionResult EditCellInfo(TrainingDayCellView trainingDayCellView)
        {
            if (ModelState.IsValid)
            {
                var trainingDayCell = (TrainingDayCell)ModelMapper.Map(trainingDayCellView, typeof(TrainingDayCellView), typeof(TrainingDayCell));

                if (trainingDayCell.ID == 0)
                {
                    Repository.CreateTrainingDayCell(trainingDayCell);
                }
                else
                {
                    Repository.UpdateTrainingDayCell(trainingDayCell);
                }
            }
            return Json(new { result = "ok" });
        }

        [HttpGet]
        public ActionResult EditCellExercise(int id, int trainingDayID)
        {
            var trainingDayCell = Repository.TrainingDayCells.FirstOrDefault(p => p.CellID == id && p.TrainingDayID == trainingDayID);
            if (trainingDayCell != null)
            {
                var trainingDayCellView = (TrainingDayCellView)ModelMapper.Map(trainingDayCell, typeof(TrainingDayCell), typeof(TrainingDayCellView));
                return View(trainingDayCellView);
            }
            else
            {
                var trainingDayCellView = new TrainingDayCellView()
                {
                    CellID = id,
                    TrainingDayID = trainingDayID
                };
                return View(trainingDayCellView);
            }
        }

        [HttpGet]
        public ActionResult EditCellValue(int id, int trainingDayID)
        {
            var trainingDayCell = Repository.TrainingDayCells.FirstOrDefault(p => p.CellID == id && p.TrainingDayID == trainingDayID);
            if (trainingDayCell != null)
            {
                var trainingDayCellView = (TrainingDayCellView)ModelMapper.Map(trainingDayCell, typeof(TrainingDayCell), typeof(TrainingDayCellView));
                return View(trainingDayCellView);
            }
            else
            {
                var trainingDayCellView = new TrainingDayCellView()
                {
                    CellID = id,
                    TrainingDayID = trainingDayID
                };
                return View(trainingDayCellView);
            }
        }

        [HttpPost]
        public ActionResult EditCellExercise(TrainingDayCellView trainingDayCellView)
        {
            if (ModelState.IsValid)
            {
                var trainingDayCell = (TrainingDayCell)ModelMapper.Map(trainingDayCellView, typeof(TrainingDayCellView), typeof(TrainingDayCell));

                if (trainingDayCell.ID == 0)
                {
                    Repository.CreateTrainingDayCell(trainingDayCell);
                }
                else
                {
                    Repository.UpdateTrainingDayCell(trainingDayCell);
                }
            }
            return Json(new { result = "ok" });
        }

        [HttpPost]
        public ActionResult ClearCell(int cellID, int trainingDayID) 
        {
            var trainingDayCell = Repository.TrainingDayCells.FirstOrDefault(p => p.CellID == cellID && p.TrainingDayID == trainingDayID);
            if (trainingDayCell != null)
            {
                Repository.RemoveTrainingDayCell(trainingDayCell.ID);
            }
            return Json(new { result = "ok" });
        }

        public ActionResult GetCellInfo(int cellID, int trainingDayID)
        {
            var trainingDayCell = Repository.TrainingDayCells.FirstOrDefault(p => p.CellID == cellID && p.TrainingDayID == trainingDayID);

            if (trainingDayCell != null)
            {
                return Json(new { result = "ok", data = trainingDayCell.Value }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "error" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult EditBatch(int id)
        {
            var trainingDay = Repository.TrainingDays.FirstOrDefault(p => p.ID == id);

            if (trainingDay != null)
            {
                var trainingDayView = (TrainingDayView)ModelMapper.Map(trainingDay, typeof(TrainingDay), typeof(TrainingDayView));
                return View(trainingDayView);
            }

            return RedirectToNotFoundPage;
        }

        [HttpPost]
        public ActionResult EditBatch(TrainingDayView trainingDayView)
        {
            var trainingDay = Repository.TrainingDays.FirstOrDefault(p => p.ID == trainingDayView.ID);

            if (trainingDay != null)
            {
                var mainTemplateArray = trainingDayView.Batch.Split(new string[] { Environment.NewLine, "\t" }, StringSplitOptions.None);
                var sb = new StringBuilder();
                int i = 0;
                foreach (var item in mainTemplateArray)
                {
                    sb.AppendFormat("{0} : {1}", i, item);
                    sb.Append(Environment.NewLine);
                    i++;
                }
                trainingDayView.Batch = sb.ToString();
                if (trainingDay.Week.Phase.Cycle.Season.Type == (int)Season.TypeEnum.OffSeason)
                {
                    SaveCell(trainingDay, mainTemplateArray, 20, 74);
                    SaveCell(trainingDay, mainTemplateArray, 21, 9);
                    SaveCell(trainingDay, mainTemplateArray, 22, 84);
                    SaveCell(trainingDay, mainTemplateArray, 23, 10);
                    SaveCell(trainingDay, mainTemplateArray, 24, 76);
                    SaveCell(trainingDay, mainTemplateArray, 25, 33);
                    SaveCell(trainingDay, mainTemplateArray, 26, 96);
                    SaveCell(trainingDay, mainTemplateArray, 27, 34);
                    SaveCell(trainingDay, mainTemplateArray, 28, 80);
                    SaveCell(trainingDay, mainTemplateArray, 29, 57);
                    SaveCell(trainingDay, mainTemplateArray, 30, 108);
                    SaveCell(trainingDay, mainTemplateArray, 31, 58);
                    SaveCell(trainingDay, mainTemplateArray, 32, 69);
                    SaveCell(trainingDay, mainTemplateArray, 37, 11);
                    SaveCell(trainingDay, mainTemplateArray, 38, 85);
                    SaveCell(trainingDay, mainTemplateArray, 39, 12);
                    SaveCell(trainingDay, mainTemplateArray, 41, 35);
                    SaveCell(trainingDay, mainTemplateArray, 42, 97);
                    SaveCell(trainingDay, mainTemplateArray, 43, 36);
                    SaveCell(trainingDay, mainTemplateArray, 45, 59);
                    SaveCell(trainingDay, mainTemplateArray, 46, 109);
                    SaveCell(trainingDay, mainTemplateArray, 47, 60);
                    SaveCell(trainingDay, mainTemplateArray, 53, 13);
                    SaveCell(trainingDay, mainTemplateArray, 54, 86);
                    SaveCell(trainingDay, mainTemplateArray, 55, 14);
                    SaveCell(trainingDay, mainTemplateArray, 57, 37);
                    SaveCell(trainingDay, mainTemplateArray, 58, 98);
                    SaveCell(trainingDay, mainTemplateArray, 59, 38);
                    SaveCell(trainingDay, mainTemplateArray, 60, 81);
                    SaveCell(trainingDay, mainTemplateArray, 61, 61);
                    SaveCell(trainingDay, mainTemplateArray, 62, 110);
                    SaveCell(trainingDay, mainTemplateArray, 63, 62);
                    SaveCell(trainingDay, mainTemplateArray, 69, 15);
                    SaveCell(trainingDay, mainTemplateArray, 70, 87);
                    SaveCell(trainingDay, mainTemplateArray, 71, 16);
                    SaveCell(trainingDay, mainTemplateArray, 72, 77);
                    SaveCell(trainingDay, mainTemplateArray, 73, 39);
                    SaveCell(trainingDay, mainTemplateArray, 74, 99);
                    SaveCell(trainingDay, mainTemplateArray, 75, 40);
                    SaveCell(trainingDay, mainTemplateArray, 77, 63);
                    SaveCell(trainingDay, mainTemplateArray, 78, 111);
                    SaveCell(trainingDay, mainTemplateArray, 79, 64);
                    SaveCell(trainingDay, mainTemplateArray, 80, 70);
                    SaveCell(trainingDay, mainTemplateArray, 82, 1);
                    SaveCell(trainingDay, mainTemplateArray, 83, 2);
                    SaveCell(trainingDay, mainTemplateArray, 85, 17);
                    SaveCell(trainingDay, mainTemplateArray, 86, 88);
                    SaveCell(trainingDay, mainTemplateArray, 87, 18);
                    SaveCell(trainingDay, mainTemplateArray, 89, 41);
                    SaveCell(trainingDay, mainTemplateArray, 90, 100);
                    SaveCell(trainingDay, mainTemplateArray, 91, 42);
                    SaveCell(trainingDay, mainTemplateArray, 92, 82);
                    SaveCell(trainingDay, mainTemplateArray, 93, 65);
                    SaveCell(trainingDay, mainTemplateArray, 94, 112);
                    SaveCell(trainingDay, mainTemplateArray, 95, 66);
                    SaveCell(trainingDay, mainTemplateArray, 96, 71);
                    SaveCell(trainingDay, mainTemplateArray, 98, 3);
                    SaveCell(trainingDay, mainTemplateArray, 99, 4);
                    SaveCell(trainingDay, mainTemplateArray, 101, 19);
                    SaveCell(trainingDay, mainTemplateArray, 102, 89);
                    SaveCell(trainingDay, mainTemplateArray, 103, 20);
                    SaveCell(trainingDay, mainTemplateArray, 105, 43);
                    SaveCell(trainingDay, mainTemplateArray, 106, 101);
                    SaveCell(trainingDay, mainTemplateArray, 107, 44);
                    SaveCell(trainingDay, mainTemplateArray, 109, 67);
                    SaveCell(trainingDay, mainTemplateArray, 110, 113);
                    SaveCell(trainingDay, mainTemplateArray, 111, 68);
                    SaveCell(trainingDay, mainTemplateArray, 112, 72);
                    SaveCell(trainingDay, mainTemplateArray, 114, 5);
                    SaveCell(trainingDay, mainTemplateArray, 115, 6);
                    SaveCell(trainingDay, mainTemplateArray, 117, 21);
                    SaveCell(trainingDay, mainTemplateArray, 118, 90);
                    SaveCell(trainingDay, mainTemplateArray, 119, 22);
                    SaveCell(trainingDay, mainTemplateArray, 120, 78);
                    SaveCell(trainingDay, mainTemplateArray, 121, 45);
                    SaveCell(trainingDay, mainTemplateArray, 122, 102);
                    SaveCell(trainingDay, mainTemplateArray, 123, 46);
                    SaveCell(trainingDay, mainTemplateArray, 124, 83);
                    SaveCell(trainingDay, mainTemplateArray, 128, 73);
                    SaveCell(trainingDay, mainTemplateArray, 130, 7);
                    SaveCell(trainingDay, mainTemplateArray, 131, 8);
                    SaveCell(trainingDay, mainTemplateArray, 133, 23);
                    SaveCell(trainingDay, mainTemplateArray, 134, 91);
                    SaveCell(trainingDay, mainTemplateArray, 135, 24);
                    SaveCell(trainingDay, mainTemplateArray, 137, 47);
                    SaveCell(trainingDay, mainTemplateArray, 138, 103);
                    SaveCell(trainingDay, mainTemplateArray, 139, 48);
                    SaveCell(trainingDay, mainTemplateArray, 148, 75);
                    SaveCell(trainingDay, mainTemplateArray, 149, 25);
                    SaveCell(trainingDay, mainTemplateArray, 150, 92);
                    SaveCell(trainingDay, mainTemplateArray, 151, 26);
                    SaveCell(trainingDay, mainTemplateArray, 153, 49);
                    SaveCell(trainingDay, mainTemplateArray, 154, 104);
                    SaveCell(trainingDay, mainTemplateArray, 155, 50);
                    SaveCell(trainingDay, mainTemplateArray, 165, 27);
                    SaveCell(trainingDay, mainTemplateArray, 166, 93);
                    SaveCell(trainingDay, mainTemplateArray, 167, 28);
                    SaveCell(trainingDay, mainTemplateArray, 168, 79);
                    SaveCell(trainingDay, mainTemplateArray, 169, 51);
                    SaveCell(trainingDay, mainTemplateArray, 170, 105);
                    SaveCell(trainingDay, mainTemplateArray, 171, 52);
                    SaveCell(trainingDay, mainTemplateArray, 181, 29);
                    SaveCell(trainingDay, mainTemplateArray, 182, 94);
                    SaveCell(trainingDay, mainTemplateArray, 183, 30);
                    SaveCell(trainingDay, mainTemplateArray, 185, 53);
                    SaveCell(trainingDay, mainTemplateArray, 186, 106);
                    SaveCell(trainingDay, mainTemplateArray, 187, 54);
                    SaveCell(trainingDay, mainTemplateArray, 197, 31);
                    SaveCell(trainingDay, mainTemplateArray, 198, 95);
                    SaveCell(trainingDay, mainTemplateArray, 199, 32);
                    SaveCell(trainingDay, mainTemplateArray, 201, 55);
                    SaveCell(trainingDay, mainTemplateArray, 202, 107);
                    SaveCell(trainingDay, mainTemplateArray, 203, 56);
                }
                else
                {
                    SaveCell(trainingDay, mainTemplateArray, 16, 74);
                    SaveCell(trainingDay, mainTemplateArray, 17, 9);
                    SaveCell(trainingDay, mainTemplateArray, 18, 84);
                    SaveCell(trainingDay, mainTemplateArray, 19, 10);
                    SaveCell(trainingDay, mainTemplateArray, 20, 76);
                    SaveCell(trainingDay, mainTemplateArray, 21, 33);
                    SaveCell(trainingDay, mainTemplateArray, 22, 96);
                    SaveCell(trainingDay, mainTemplateArray, 23, 34);
                    SaveCell(trainingDay, mainTemplateArray, 24, 69);
                    SaveCell(trainingDay, mainTemplateArray, 29, 11);
                    SaveCell(trainingDay, mainTemplateArray, 30, 85);
                    SaveCell(trainingDay, mainTemplateArray, 31, 12);
                    SaveCell(trainingDay, mainTemplateArray, 33, 35);
                    SaveCell(trainingDay, mainTemplateArray, 34, 97);
                    SaveCell(trainingDay, mainTemplateArray, 35, 36);
                    SaveCell(trainingDay, mainTemplateArray, 41, 13);
                    SaveCell(trainingDay, mainTemplateArray, 42, 86);
                    SaveCell(trainingDay, mainTemplateArray, 43, 14);
                    SaveCell(trainingDay, mainTemplateArray, 45, 37);
                    SaveCell(trainingDay, mainTemplateArray, 46, 98);
                    SaveCell(trainingDay, mainTemplateArray, 47, 38);
                    SaveCell(trainingDay, mainTemplateArray, 53, 15);
                    SaveCell(trainingDay, mainTemplateArray, 54, 87);
                    SaveCell(trainingDay, mainTemplateArray, 55, 16);
                    SaveCell(trainingDay, mainTemplateArray, 56, 77);
                    SaveCell(trainingDay, mainTemplateArray, 57, 39);
                    SaveCell(trainingDay, mainTemplateArray, 58, 99);
                    SaveCell(trainingDay, mainTemplateArray, 59, 40);
                    SaveCell(trainingDay, mainTemplateArray, 60, 70);
                    SaveCell(trainingDay, mainTemplateArray, 62, 1);
                    SaveCell(trainingDay, mainTemplateArray, 63, 2);
                    SaveCell(trainingDay, mainTemplateArray, 65, 17);
                    SaveCell(trainingDay, mainTemplateArray, 66, 88);
                    SaveCell(trainingDay, mainTemplateArray, 67, 18);
                    SaveCell(trainingDay, mainTemplateArray, 69, 41);
                    SaveCell(trainingDay, mainTemplateArray, 70, 100);
                    SaveCell(trainingDay, mainTemplateArray, 71, 42);
                    SaveCell(trainingDay, mainTemplateArray, 72, 71);
                    SaveCell(trainingDay, mainTemplateArray, 74, 3);
                    SaveCell(trainingDay, mainTemplateArray, 75, 4);
                    SaveCell(trainingDay, mainTemplateArray, 77, 19);
                    SaveCell(trainingDay, mainTemplateArray, 78, 89);
                    SaveCell(trainingDay, mainTemplateArray, 79, 20);
                    SaveCell(trainingDay, mainTemplateArray, 81, 43);
                    SaveCell(trainingDay, mainTemplateArray, 82, 101);
                    SaveCell(trainingDay, mainTemplateArray, 83, 44);
                    SaveCell(trainingDay, mainTemplateArray, 84, 72);
                    SaveCell(trainingDay, mainTemplateArray, 86, 5);
                    SaveCell(trainingDay, mainTemplateArray, 87, 6);
                    SaveCell(trainingDay, mainTemplateArray, 89, 21);
                    SaveCell(trainingDay, mainTemplateArray, 90, 90);
                    SaveCell(trainingDay, mainTemplateArray, 91, 22);
                    SaveCell(trainingDay, mainTemplateArray, 92, 78);
                    SaveCell(trainingDay, mainTemplateArray, 93, 45);
                    SaveCell(trainingDay, mainTemplateArray, 94, 102);
                    SaveCell(trainingDay, mainTemplateArray, 95, 46);
                    SaveCell(trainingDay, mainTemplateArray, 96, 73);
                    SaveCell(trainingDay, mainTemplateArray, 98, 7);
                    SaveCell(trainingDay, mainTemplateArray, 99, 8);
                    SaveCell(trainingDay, mainTemplateArray, 101, 23);
                    SaveCell(trainingDay, mainTemplateArray, 102, 91);
                    SaveCell(trainingDay, mainTemplateArray, 103, 24);
                    SaveCell(trainingDay, mainTemplateArray, 105, 47);
                    SaveCell(trainingDay, mainTemplateArray, 106, 103);
                    SaveCell(trainingDay, mainTemplateArray, 107, 48);
                    SaveCell(trainingDay, mainTemplateArray, 112, 75);
                    SaveCell(trainingDay, mainTemplateArray, 113, 25);
                    SaveCell(trainingDay, mainTemplateArray, 114, 92);
                    SaveCell(trainingDay, mainTemplateArray, 115, 26);
                    SaveCell(trainingDay, mainTemplateArray, 117, 49);
                    SaveCell(trainingDay, mainTemplateArray, 118, 104);
                    SaveCell(trainingDay, mainTemplateArray, 119, 50);
                    SaveCell(trainingDay, mainTemplateArray, 125, 27);
                    SaveCell(trainingDay, mainTemplateArray, 126, 93);
                    SaveCell(trainingDay, mainTemplateArray, 127, 28);
                    SaveCell(trainingDay, mainTemplateArray, 128, 79);
                    SaveCell(trainingDay, mainTemplateArray, 137, 29);
                    SaveCell(trainingDay, mainTemplateArray, 138, 94);
                    SaveCell(trainingDay, mainTemplateArray, 139, 30);
                    SaveCell(trainingDay, mainTemplateArray, 149, 31);
                    SaveCell(trainingDay, mainTemplateArray, 150, 95);
                    SaveCell(trainingDay, mainTemplateArray, 151, 32);
                }
               

                return RedirectToAction("EditTable", new { id = trainingDay.ID });
            }

            return RedirectToNotFoundPage;
        }

        public void SaveCell(TrainingDay trainingDay, string[] batchArr, int num, int cellId)
        {
            logger.Debug(string.Format("Start Save Cell {0} cellID = {1}", trainingDay.ID, cellId));

            var cell = Repository.Cells.FirstOrDefault(p => p.ID == cellId);
            if (cell != null)
            {
                switch ((Cell.CellType)cell.Type)
                {
                    case Cell.CellType.Exercise:
                        SaveTraining(trainingDay, batchArr, num, cellId);
                        return;
                    case Cell.CellType.Info:
                        SaveInfo(trainingDay, batchArr, num, cellId);
                        return;
                    case Cell.CellType.Value:
                        SaveExercise(trainingDay, batchArr, num, cellId);
                        return;
                }
            }
        }

        public void SaveExercise(TrainingDay trainingDay, string[] batchArr, int num, int cellId)
        {
            logger.Debug(string.Format("Start Info {0} cellID = {1}", trainingDay.ID, cellId));
            if (batchArr.Count() > num)
            {
                var value = batchArr[num].Trim();
                
                logger.Debug(string.Format("Value {0}", value));
                if (!string.IsNullOrWhiteSpace(value))
                {
                    var trainingDayCell = Repository.TrainingDayCells.FirstOrDefault(p => p.CellID == cellId && p.TrainingDayID == trainingDay.ID);

                    if (trainingDayCell == null)
                    {
                        trainingDayCell = new TrainingDayCell()
                        {
                            TrainingDayID = trainingDay.ID,
                            CellID = cellId,

                        };
                        Repository.CreateTrainingDayCell(trainingDayCell);
                    }
                    int intValue = 0;
                    if (Int32.TryParse(value, out intValue))
                    {
                        var sbcType = 0;
                        double coeff = 0.0;
                        if (intValue % 101 == 0)
                        {
                            sbcType = (int)TrainingDayCell.SBCTypeEnum.S;
                            coeff = (double)intValue / 101000;
                        }
                        if (intValue % 103 == 0)
                        {
                            sbcType = (int)TrainingDayCell.SBCTypeEnum.B;
                            coeff = (double)intValue / 103000;
                        }
                        if (intValue % 107 == 0)
                        {
                            sbcType = (int)TrainingDayCell.SBCTypeEnum.C;
                            coeff = (double)intValue / 107000;
                        }

                        trainingDayCell.SBCType = sbcType;
                        trainingDayCell.Coefficient = coeff;
                    }
                    else
                    {
                        trainingDayCell.PrimaryText = value;
                    }
                    Repository.UpdateTrainingDayCell(trainingDayCell);
                }
            }
            else
            {
                logger.Debug("Value NULL");
            }
        }

        public void SaveInfo(TrainingDay trainingDay, string[] batchArr, int num, int cellId)
        {
            logger.Debug(string.Format("Start Info {0} cellID = {1}", trainingDay.ID, cellId));
            if (batchArr.Count() > num)
            {
                var value = batchArr[num].Trim();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    logger.Debug(string.Format("Value {0}", value));

                    var trainingDayCell = Repository.TrainingDayCells.FirstOrDefault(p => p.CellID == cellId && p.TrainingDayID == trainingDay.ID);

                    if (trainingDayCell == null)
                    {
                        trainingDayCell = new TrainingDayCell()
                        {
                            TrainingDayID = trainingDay.ID,
                            CellID = cellId,

                        };
                        Repository.CreateTrainingDayCell(trainingDayCell);
                    }
                    trainingDayCell.PrimaryText = value;
                    Repository.UpdateTrainingDayCell(trainingDayCell);
                }
            }
            else
            {
                logger.Debug("Value NULL");
            }
        }

        public void SaveTraining(TrainingDay trainingDay, string[] batchArr, int num, int cellId)
        {
            logger.Debug(string.Format("Start TrainingDay {0} cellID = {1}", trainingDay.ID, cellId));
            if (batchArr.Count() > num)
            {
                var value = batchArr[num].Trim();
                logger.Debug(string.Format("Value {0}", value));

                var trainingSets = Repository.TrainingSets.Where(p => p.DayID == trainingDay.DayID && p.PhaseID == trainingDay.Week.PhaseID);
                var trainingSet = trainingSets.FirstOrDefault(p => p.TrainingEquipments.Any(r => string.Compare(r.Training.Name, value, true) == 0));
                if (trainingSet != null)
                {
                    var trainingDayCell = Repository.TrainingDayCells.FirstOrDefault(p => p.CellID == cellId && p.TrainingDayID == trainingDay.ID);

                    if (trainingDayCell == null)
                    {
                        trainingDayCell = new TrainingDayCell()
                        {
                            TrainingDayID = trainingDay.ID,
                            CellID = cellId,
                        };
                        Repository.CreateTrainingDayCell(trainingDayCell);
                    }
                    trainingDayCell.TrainingSetID = trainingSet.ID;
                    Repository.UpdateTrainingDayCell(trainingDayCell);
                }
                else
                {
                    logger.Debug(string.Format("NOT FOUND Training {0}", value));
                }
            }
            else
            {
                logger.Debug("Value NULL");
            }
        }
    }
}
