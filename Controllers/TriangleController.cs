using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TriangleLab.Models;

namespace TriangleLab.Controllers
{
    [Route("triangle/[action]")]
    public class TriangleController : Controller
    {
        private IActionResult ValidateAndExecute(Triangle triangle, Func<IActionResult> action)
        {
            if (triangle == null || !triangle.IsValid())
            {
                return Content("Error: Invalid triangle! (e.g. violation of triangle inequality or non-positive sides)", "text/plain; charset=utf-8");
            }
            return action();
        }

        [HttpGet]
        public IActionResult Info(double side1, double side2, double side3)
        {
            var t = new Triangle { Side1 = side1, Side2 = side2, Side3 = side3 };
            return ValidateAndExecute(t, () =>
            {
                double[] sides = { side1, side2, side3 };
                Array.Sort(sides);
                double perimeter = t.GetPerimeter();
                double area = t.GetArea();

                string result = $"Triangle:\n" +
                                $"({sides[0]}, {sides[1]}, {sides[2]})\n" +
                                $"Reduced:\n" +
                                $"({sides[0] / perimeter:R}, {sides[1] / perimeter:R}, {sides[2] / perimeter:R})\n\n" +
                                $"Area = {area}\n" +
                                $"Perimeter = {perimeter}";

                return Content(result, "text/plain; charset=utf-8");
            });
        }

        [HttpGet]
        public IActionResult Area(double side1, double side2, double side3)
        {
            var t = new Triangle { Side1 = side1, Side2 = side2, Side3 = side3 };
            return ValidateAndExecute(t, () => Content(t.GetArea().ToString(System.Globalization.CultureInfo.InvariantCulture)));
        }

        [HttpGet]
        public IActionResult Perimeter(double side1, double side2, double side3)
        {
            var t = new Triangle { Side1 = side1, Side2 = side2, Side3 = side3 };
            return ValidateAndExecute(t, () => Content(t.GetPerimeter().ToString(System.Globalization.CultureInfo.InvariantCulture)));
        }

        [HttpGet]
        public IActionResult IsRightAngled(double side1, double side2, double side3)
        {
            var t = new Triangle { Side1 = side1, Side2 = side2, Side3 = side3 };
            return ValidateAndExecute(t, () => Content(t.IsRightAngled().ToString().ToLower()));
        }

        [HttpGet]
        public IActionResult IsEquilateral(double side1, double side2, double side3)
        {
            var t = new Triangle { Side1 = side1, Side2 = side2, Side3 = side3 };
            return ValidateAndExecute(t, () => Content(t.IsEquilateral().ToString().ToLower()));
        }

        [HttpGet]
        public IActionResult IsIsosceles(double side1, double side2, double side3)
        {
            var t = new Triangle { Side1 = side1, Side2 = side2, Side3 = side3 };
            return ValidateAndExecute(t, () => Content(t.IsIsosceles().ToString().ToLower()));
        }

        [HttpGet]
        public IActionResult AreCongruent(Triangle tr1, Triangle tr2)
        {
            if (tr1 == null || tr2 == null || !tr1.IsValid() || !tr2.IsValid())
                return Content("Error: Invalid triangles!", "text/plain; charset=utf-8");

            return Content(tr1.IsCongruentTo(tr2).ToString().ToLower());
        }

        [HttpGet]
        public IActionResult AreSimilar(Triangle tr1, Triangle tr2)
        {
            if (tr1 == null || tr2 == null || !tr1.IsValid() || !tr2.IsValid())
                return Content("Error: Invalid triangles!", "text/plain; charset=utf-8");

            return Content(tr1.IsSimilarTo(tr2).ToString().ToLower());
        }

        [HttpGet]
        public IActionResult GreatestByPerimeter(Triangle[] tr)
        {
            if (tr == null || tr.Length == 0 || tr.Any(t => t == null || !t.IsValid()))
                return Content("Error: Invalid triangles array!", "text/plain; charset=utf-8");

            var maxT = tr.OrderByDescending(t => t.GetPerimeter()).First();
            return RedirectToAction("Info", new { side1 = maxT.Side1, side2 = maxT.Side2, side3 = maxT.Side3 });
        }

        [HttpGet]
        public IActionResult GreatestByArea(Triangle[] tr)
        {
            if (tr == null || tr.Length == 0 || tr.Any(t => t == null || !t.IsValid()))
                return Content("Error: Invalid triangles array!", "text/plain; charset=utf-8");

            var maxT = tr.OrderByDescending(t => t.GetArea()).First();
            return RedirectToAction("Info", new { side1 = maxT.Side1, side2 = maxT.Side2, side3 = maxT.Side3 });
        }

        [HttpGet]
        public IActionResult PairwiseNonSimilar(Triangle[] tr)
        {
            if (tr == null || tr.Length == 0 || tr.Any(t => t == null || !t.IsValid()))
                return Content("Error: Invalid triangles array!", "text/plain; charset=utf-8");

            var nonSimilar = new System.Collections.Generic.List<Triangle>();
            foreach (var t in tr)
            {
                if (!nonSimilar.Any(ns => ns.IsSimilarTo(t)))
                {
                    nonSimilar.Add(t);
                }
            }

            string output = string.Join("\n========================\n", nonSimilar.Select(t =>
                $"Triangle: ({t.Side1}, {t.Side2}, {t.Side3})\nArea = {t.GetArea()}\nPerimeter = {t.GetPerimeter()}"));

            return Content(output, "text/plain; charset=utf-8");
        }
    }
}