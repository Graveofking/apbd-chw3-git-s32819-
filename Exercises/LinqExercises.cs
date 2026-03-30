using System.Collections;
using LinqConsoleLab.EN.Data;
using LinqConsoleLab.EN.Models;
namespace LinqConsoleLab.EN.Exercises;

public sealed class LinqExercises
{
    /// <summary>
    /// Task:
    /// Find all students who live in Warsaw.
    /// Return the index number, full name, and city.
    ///
    /// SQL:
    /// SELECT IndexNumber, FirstName, LastName, City
    /// FROM Students
    /// WHERE City = 'Warsaw';
    /// </summary>
    public IEnumerable<string> Task01_StudentsFromWarsaw()
    {
       //Query
       var query=
           from s in UniversityData.Students
           where s.City.Equals("Warsaw")
           select $"{s.IndexNumber},{s.FirstName},{s.LastName},{s.City}";
       var method= UniversityData.Students
           .Where(s => s.City.Equals("Warsaw"))
           .Select(s => $"{s.IndexNumber},{s.FirstName},{s.LastName},{s.City}");
       return query;
    }

    /// <summary>
    /// Task:
    /// Build a list of all student email addresses.
    /// Use projection so that you do not return whole objects.
    ///
    /// SQL:
    /// SELECT Email
    /// FROM Students;
    /// </summary>
    public IEnumerable<string> Task02_StudentEmailAddresses()
    {

        var res = UniversityData.Students
            .Join(UniversityData.Enrollments, s => s.Id,
                e => e.StudentId,
                (s, e) => new
                {
                    s.FirstName, s.LastName, e.EnrollmentDate
                });
        return res.Select(s=>$"{s.FirstName},{s.LastName},{s.EnrollmentDate}");
    }

    /// <summary>
    /// Task:
    /// Sort students alphabetically by last name and then by first name.
    /// Return the index number and full name.
    ///
    /// SQL:
    /// SELECT IndexNumber, FirstName, LastName
    /// FROM Students
    /// ORDER BY LastName, FirstName;
    /// </summary>
    public IEnumerable<string> Task03_StudentsSortedAlphabetically()
    {
        var res = from s in UniversityData.Students
            orderby s.LastName, s.FirstName
            select $"{s.IndexNumber},{s.FirstName},{s.LastName},{s.IndexNumber}";
        
        return res; 
    }

    /// <summary>
    /// Task:
    /// Find the first course from the Analytics category.
    /// If such a course does not exist, return a text message.
    ///
    /// SQL:
    /// SELECT TOP 1 Title, StartDate
    /// FROM Courses
    /// WHERE Category = 'Analytics';
    /// </summary>
    public IEnumerable<string> Task04_FirstAnalyticsCourse()
    {
        var course = UniversityData.Courses.FirstOrDefault(c => c.Category == "Analytics");

        if (course != null)
        {
            yield return $"{course.Title} (Start Date: {course.StartDate:yyyy-MM-dd})";
        }
        else
        {
            yield return "No Analytics course found.";
        }
    }

    /// <summary>
    /// Task:
    /// Check whether there is at least one inactive enrollment in the data set.
    /// Return one line with a True/False or Yes/No answer.
    ///
    /// SQL:
    /// SELECT CASE WHEN EXISTS (
    ///     SELECT 1
    ///     FROM Enrollments
    ///     WHERE IsActive = 0
    /// ) THEN 1 ELSE 0 END;
    /// </summary>
    public IEnumerable<string> Task05_IsThereAnyInactiveEnrollment()
    {
        bool hasInactiveEnrollments = UniversityData.Enrollments.Any(e => !e.IsActive);
        
        yield return hasInactiveEnrollments ? "Yes" : "No";
    }

    /// <summary>
    /// Task:
    /// Check whether every lecturer has a department assigned.
    /// Use a method that validates the condition for the whole collection.
    ///
    /// SQL:
    /// SELECT CASE WHEN COUNT(*) = COUNT(Department)
    /// THEN 1 ELSE 0 END
    /// FROM Lecturers;
    /// </summary>
    public IEnumerable<string> Task06_DoAllLecturersHaveDepartment()
    {
        bool allHaveDepartment = UniversityData.Lecturers.All(l => !string.IsNullOrWhiteSpace(l.Department));
        
        yield return allHaveDepartment ? "Yes" : "No";
    }

    /// <summary>
    /// Task:
    /// Count how many active enrollments exist in the system.
    ///
    /// SQL:
    /// SELECT COUNT(*)
    /// FROM Enrollments
    /// WHERE IsActive = 1;
    /// </summary>
    public IEnumerable<string> Task07_CountActiveEnrollments()
    {
        int activeCount = UniversityData.Enrollments.Count(e => e.IsActive);
        
        yield return activeCount.ToString();
    }

    /// <summary>
    /// Task:
    /// Return a sorted list of distinct student cities.
    ///
    /// SQL:
    /// SELECT DISTINCT City
    /// FROM Students
    /// ORDER BY City;
    /// </summary>
    public IEnumerable<string> Task08_DistinctStudentCities()
    {
        var distinctCities = UniversityData.Students
            .Select(s => s.City)
            .Distinct()
            .OrderBy(city => city);

        foreach (var city in distinctCities)
        {
            yield return city;
        }
    }

    /// <summary>
    /// Task:
    /// Return the three newest enrollments.
    /// Show the enrollment date, student identifier, and course identifier.
    ///
    /// SQL:
    /// SELECT TOP 3 EnrollmentDate, StudentId, CourseId
    /// FROM Enrollments
    /// ORDER BY EnrollmentDate DESC;
    /// </summary>
    public IEnumerable<string> Task09_ThreeNewestEnrollments()
    {
        var newestEnrollments = UniversityData.Enrollments
            .OrderByDescending(e => e.EnrollmentDate)
            .Take(3);

        foreach (var enrollment in newestEnrollments)
        {
            yield return $"Date: {enrollment.EnrollmentDate:yyyy-MM-dd} | Student ID: {enrollment.StudentId} | Course ID: {enrollment.CourseId}";
        }
    }

    /// <summary>
    /// Task:
    /// Implement simple pagination for the course list.
    /// Assume a page size of 2 and return the second page of data.
    ///
    /// SQL:
    /// SELECT Title, Category
    /// FROM Courses
    /// ORDER BY Title
    /// OFFSET 2 ROWS FETCH NEXT 2 ROWS ONLY;
    /// </summary>
    public IEnumerable<string> Task10_SecondPageOfCourses()
    {
        int pageSize = 2;
        int pageNumber = 2; 
        
        var secondPageCourses = UniversityData.Courses
            .OrderBy(c => c.Title)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);

        foreach (var course in secondPageCourses)
        {
            yield return $"{course.Title} | Category: {course.Category}";
        }
    }

    /// <summary>
    /// Task:
    /// Join students with enrollments by StudentId.
    /// Return the full student name and the enrollment date.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, e.EnrollmentDate
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId;
    /// </summary>
    public IEnumerable<string> Task11_JoinStudentsWithEnrollments()
    {
        // Method syntax
        var method = UniversityData.Students
            .Join(UniversityData.Enrollments,
                s => s.Id,
                e => e.StudentId,
                (s, e) => $"{s.FirstName} {s.LastName} | Enrolled: {e.EnrollmentDate:yyyy-MM-dd}");
        return method;
        }

    /// <summary>
    /// Task:
    /// Prepare all student-course pairs based on enrollments.
    /// Use an approach that flattens the data into a single result sequence.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, c.Title
    /// FROM Enrollments e
    /// JOIN Students s ON s.Id = e.StudentId
    /// JOIN Courses c ON c.Id = e.CourseId;
    /// </summary>
    public IEnumerable<string> Task12_StudentCoursePairs()
    {
        // SelectMany flattens student → many enrollments → each matched to a course
        var result = UniversityData.Students
            .SelectMany(
                s => UniversityData.Enrollments.Where(e => e.StudentId == s.Id),
                (s, e) => new { Student = s, Enrollment = e })
            .Join(UniversityData.Courses,
                se => se.Enrollment.CourseId,
                c => c.Id,
                (se, c) => $"{se.Student.FirstName} {se.Student.LastName} | {c.Title}");
        return result;
    }

    /// <summary>
    /// Task:
    /// Group enrollments by course and return the course title together with the number of enrollments.
    ///
    /// SQL:
    /// SELECT c.Title, COUNT(*)
    /// FROM Enrollments e
    /// JOIN Courses c ON c.Id = e.CourseId
    /// GROUP BY c.Title;
    /// </summary>
    public IEnumerable<string> Task13_GroupEnrollmentsByCourse()
    {
        var result = UniversityData.Enrollments
            .Join(UniversityData.Courses,
                e => e.CourseId,
                c => c.Id,
                (e, c) => c.Title) // keep only the title we need for grouping
            .GroupBy(title => title)
            .Select(g => $"{g.Key} | Enrollments: {g.Count()}");
        return result;
    }

    /// <summary>
    /// Task:
    /// Calculate the average final grade for each course.
    /// Ignore records where the final grade is null.
    ///
    /// SQL:
    /// SELECT c.Title, AVG(e.FinalGrade)
    /// FROM Enrollments e
    /// JOIN Courses c ON c.Id = e.CourseId
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY c.Title;
    /// </summary>
    public IEnumerable<string> Task14_AverageGradePerCourse()
    {
        var result = UniversityData.Enrollments
            .Where(e => e.FinalGrade.HasValue)
            .Join(UniversityData.Courses,
                e => e.CourseId,
                c => c.Id,
                (e, c) => new { c.Title, e.FinalGrade!.Value })
            .GroupBy(x => x.Title)
            .Select(g => $"{g.Key} | Avg grade: {g.Average(x => x.Value):F2}");
        return result;
    }

    /// <summary>
    /// Task:
    /// For each lecturer, count how many courses are assigned to that lecturer.
    /// Return the full lecturer name and the course count.
    ///
    /// SQL:
    /// SELECT l.FirstName, l.LastName, COUNT(c.Id)
    /// FROM Lecturers l
    /// LEFT JOIN Courses c ON c.LecturerId = l.Id
    /// GROUP BY l.FirstName, l.LastName;
    /// </summary>
    public IEnumerable<string> Task15_LecturersAndCourseCounts()
    {
        var result = UniversityData.Lecturers
            .GroupJoin(UniversityData.Courses,
                l => l.Id,
                c => c.LecturerId,
                (l, courses) => $"{l.FirstName} {l.LastName} | Courses: {courses.Count()}");
        return result;
    }

    /// <summary>
    /// Task:
    /// For each student, find the highest final grade.
    /// Skip students who do not have any graded enrollment yet.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, MAX(e.FinalGrade)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY s.FirstName, s.LastName;
    /// </summary>
    public IEnumerable<string> Task16_HighestGradePerStudent()
    {
        var result = UniversityData.Enrollments
            .Where(e => e.FinalGrade.HasValue)
            .Join(UniversityData.Students,
                e => e.StudentId,
                s => s.Id,
                (e, s) => new { s.FirstName, s.LastName, e.FinalGrade!.Value })
            .GroupBy(x => new { x.FirstName, x.LastName })
            .Select(g => $"{g.Key.FirstName} {g.Key.LastName} | Best grade: {g.Max(x => x.Value):F2}");
        return result;
    }

    /// <summary>
    /// Challenge:
    /// Find students who have more than one active enrollment.
    /// Return the full name and the number of active courses.
    ///
    /// SQL:
    /// SELECT s.FirstName, s.LastName, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.FirstName, s.LastName
    /// HAVING COUNT(*) > 1;
    /// </summary>
    public IEnumerable<string> Challenge01_StudentsWithMoreThanOneActiveCourse()
    {
        var result = UniversityData.Enrollments
            .Where(e => e.IsActive)
            .Join(UniversityData.Students,
                e => e.StudentId,
                s => s.Id,
                (e, s) => new { s.FirstName, s.LastName })
            .GroupBy(x => new { x.FirstName, x.LastName })
            .Where(g => g.Count() > 1)
            .Select(g => $"{g.Key.FirstName} {g.Key.LastName} | Active courses: {g.Count()}");
        return result;
    }

    /// <summary>
    /// Challenge:
    /// List the courses that start in April 2026 and do not have any final grades assigned yet.
    ///
    /// SQL:
    /// SELECT c.Title
    /// FROM Courses c
    /// JOIN Enrollments e ON c.Id = e.CourseId
    /// WHERE MONTH(c.StartDate) = 4 AND YEAR(c.StartDate) = 2026
    /// GROUP BY c.Title
    /// HAVING SUM(CASE WHEN e.FinalGrade IS NOT NULL THEN 1 ELSE 0 END) = 0;
    /// </summary>
    public IEnumerable<string> Challenge02_AprilCoursesWithoutFinalGrades()
    {
        var result = UniversityData.Courses
            .Where(c => c.StartDate.Year == 2026 && c.StartDate.Month == 4)
            .GroupJoin(UniversityData.Enrollments,
                c => c.Id,
                e => e.CourseId,
                (c, enrollments) => new { c.Title, Enrollments = enrollments })
            .Where(x => !x.Enrollments.Any(e => e.FinalGrade.HasValue))
            .Select(x => x.Title);
        return result;
    }

    /// <summary>
    /// Challenge:
    /// Calculate the average final grade for every lecturer across all of their courses.
    /// Ignore missing grades but still keep the lecturers in mind as the reporting dimension.
    ///
    /// SQL:
    /// SELECT l.FirstName, l.LastName, AVG(e.FinalGrade)
    /// FROM Lecturers l
    /// LEFT JOIN Courses c ON c.LecturerId = l.Id
    /// LEFT JOIN Enrollments e ON e.CourseId = c.Id
    /// WHERE e.FinalGrade IS NOT NULL
    /// GROUP BY l.FirstName, l.LastName;
    /// </summary>
    public IEnumerable<string> Challenge03_LecturersAndAverageGradeAcrossTheirCourses()
    {
        var result = UniversityData.Lecturers
            .Join(UniversityData.Courses,
                l => l.Id,
                c => c.LecturerId,
                (l, c) => new { Lecturer = l, Course = c })
            .Join(UniversityData.Enrollments,
                lc => lc.Course.Id,
                e  => e.CourseId,
                (lc, e) => new { lc.Lecturer.FirstName, lc.Lecturer.LastName, e.FinalGrade })
            .Where(x => x.FinalGrade.HasValue)
            .GroupBy(x => new { x.FirstName, x.LastName })
            .Select(g => $"{g.Key.FirstName} {g.Key.LastName} | Avg grade: {g.Average(x => x.FinalGrade!.Value):F2}");
        return result;
    }

    /// <summary>
    /// Challenge:
    /// Show student cities and the number of active enrollments created by students from each city.
    /// Sort the result by the active enrollment count in descending order.
    ///
    /// SQL:
    /// SELECT s.City, COUNT(*)
    /// FROM Students s
    /// JOIN Enrollments e ON s.Id = e.StudentId
    /// WHERE e.IsActive = 1
    /// GROUP BY s.City
    /// ORDER BY COUNT(*) DESC;
    /// </summary>
    public IEnumerable<string> Challenge04_CitiesAndActiveEnrollmentCounts()
    {
        var result = UniversityData.Enrollments
            .Where(e => e.IsActive)
            .Join(UniversityData.Students,
                e => e.StudentId,
                s => s.Id,
                (e, s) => s.City)
            .GroupBy(city => city)
            .OrderByDescending(g => g.Count())
            .Select(g => $"{g.Key} | Active enrollments: {g.Count()}");
        return result;
    }

    private static NotImplementedException NotImplemented(string methodName)
    {
        return new NotImplementedException(
            $"Complete method {methodName} in Exercises/LinqExercises.cs and run the command again.");
    }
}
