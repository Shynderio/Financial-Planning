using System;
using System.Threading.Tasks;
using FinancialPlanning.Service.Services;
using FinancialPlanning.Data.Entities;
using FinancialPlanning.Data.Repositories;
using Moq;
using Xunit;

namespace TestProject.Service.Services
{
    public class TermServiceTests
    {
        private Term _testTerm = new Term
        {
            Id = Guid.NewGuid(),
            TermName = "Test Term",
            StartDate = DateTime.Now,
            Duration = 6, // 6 months
            ReportDueDate = DateTime.Now.AddDays(20),
            PlanDueDate = DateTime.Now.AddDays(30)
        };

        [Fact]
        public async Task GetStartingTerms_ReturnsStartingTerms()
        {
            // Arrange
            var mockRepository = new Mock<ITermRepository>();
            var termService = new TermService(mockRepository.Object);

            var startDate = DateTime.Now.AddDays(-5); // Start date within the past 7 days
            var startingTerm = new Term { StartDate = startDate, Status = 1, Id = Guid.NewGuid() };
            var endingTerm = new Term { StartDate = startDate.AddDays(-9), Status = 1, Id = Guid.NewGuid() }; // Not within the past 7 days

            mockRepository.Setup(repo => repo.GetAllTerms()).ReturnsAsync(new List<Term> { startingTerm, endingTerm });

            // Act
            var result = await termService.GetStartingTerms();

            // Assert
            Assert.Contains(startingTerm, result);
            Assert.DoesNotContain(result, term => term.Id == endingTerm.Id);
        }

        [Fact]
        public async Task StartTerm_UpdatesTermStatus()
        {
            // Arrange
            var mockRepository = new Mock<ITermRepository>();
            var termService = new TermService(mockRepository.Object);
            mockRepository.Setup(repo => repo.GetTermById(_testTerm.Id)).ReturnsAsync(_testTerm);
            // Act
            await termService.StartTerm(_testTerm.Id);

            // Assert
            Assert.Equal(2, _testTerm.Status);
            mockRepository.Verify(repo => repo.UpdateTerm(_testTerm), Times.Once);
        }

        // [Fact]
        // public async Task CreateTerm_CreatesNewTerm_AutomaticallySetStatus()
        // {
        //     // Arrange
        //     var mockRepository = new Mock<ITermRepository>();
        //     var termService = new TermService(mockRepository.Object);

        //     // Act
        //     await termService.CreateTerm(_testTerm);

        //     // Assert
        //     Assert.Equal(1, _testTerm.Status);
        //     mockRepository.Verify(repo => repo.CreateTerm(_testTerm), Times.Once);
        // }

        [Fact]
        public async Task CreateTerm_Successfully_Creates_Term()
        {
            // Arrange
            var mockRepository = new Mock<ITermRepository>();
            var service = new TermService(mockRepository.Object);

            // Act
            await service.CreateTerm(_testTerm);

            // Assert
            mockRepository.Verify(repo => repo.CreateTerm(_testTerm), Times.Once);
        }

        [Fact]
        public async Task CreateTerm_CreatesNewTerm_AutomaticallySetStatus()
        {
            // Arrange
            var mockRepository = new Mock<ITermRepository>();
            var termService = new TermService(mockRepository.Object);

            var term = new Term();
            term.Status = 0;
            mockRepository.Setup(repo => repo.CreateTerm(It.IsAny<Term>())).Returns(Task.FromResult(Guid.NewGuid()));

            // Act
            await termService.CreateTerm(term);

            // Assert
            Assert.Equal(1, term.Status); // Assert the status set after calling CreateTerm
            mockRepository.Verify(repo => repo.CreateTerm(term), Times.Once);
        }


        [Fact]
        public async Task CreateTerm_Throws_Exception_ReportDueDate_After_EndDate()
        {
            // Arrange
            _testTerm.ReportDueDate = DateTime.Now.AddMonths(7); // Report due date after end date
            var mockRepository = new Mock<ITermRepository>();
            var service = new TermService(mockRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateTerm(_testTerm));
        }

        [Fact]
        public async Task CreateTerm_Throws_Exception_PlanDueDate_After_EndDate()
        {
            // Arrange
            _testTerm.PlanDueDate = DateTime.Now.AddMonths(7); // Plan due date after end date
            var mockRepository = new Mock<ITermRepository>();
            var service = new TermService(mockRepository.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateTerm(_testTerm));
        }

        [Fact]
        public async Task UpdateTerm_NonExistentTerm_ShouldThrowException()
        {
            // Arrange
            var mockRepository = new Mock<ITermRepository>();
            var termService = new TermService(mockRepository.Object);
            var nonExistentTerm = new Term { Id = Guid.NewGuid(), TermName = "Non-Existent Term" };

            mockRepository.Setup(repo => repo.GetTermById(nonExistentTerm.Id)).ReturnsAsync((Term)null!);
            // Returning null as the term doesn't exist

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => termService.UpdateTerm(nonExistentTerm));
        }

        [Fact]
        public async Task UpdateTerm_UpdatesExistingTerm()
        {
            // Arrange
            var mockRepository = new Mock<ITermRepository>();
            var termService = new TermService(mockRepository.Object);
            var termToUpdate = new Term { Id = Guid.NewGuid(), TermName = "Existing Term", Status = 1};
            var updatedTerm = new Term { Id = termToUpdate.Id, TermName = "Updated Term" };

            mockRepository.Setup(repo => repo.GetTermById(termToUpdate.Id)).ReturnsAsync(termToUpdate);

            // Act
            await termService.UpdateTerm(updatedTerm);

            // Assert
            mockRepository.Verify(repo => repo.GetTermById(updatedTerm.Id), Times.Once);
            mockRepository.Verify(repo => repo.UpdateTerm(updatedTerm), Times.Once);
        }

        [Fact]
        public async Task DeleteTerm_WithValidId_DeletesTerm()
        {
            // Arrange
            var mockRepository = new Mock<ITermRepository>();
            var termService = new TermService(mockRepository.Object);
            var id = Guid.NewGuid();
            var termToDelete = new Term { Id = id };

            mockRepository.Setup(repo => repo.GetTermById(id)).ReturnsAsync(termToDelete);

            // Act
            await termService.DeleteTerm(id);

            // Assert
            mockRepository.Verify(repo => repo.DeleteTerm(termToDelete), Times.Once);
        }

        [Fact]
        public async Task DeleteTerm_WithInvalidId_ThrowsArgumentException()
        {
            // Arrange
            var mockRepository = new Mock<ITermRepository>();
            var termService = new TermService(mockRepository.Object);
            var id = Guid.NewGuid();

            mockRepository.Setup(repo => repo.GetTermById(id)).ReturnsAsync((Term)null!);
            
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => termService.DeleteTerm(id));
        }

        [Fact]
        public async Task CloseTerms_ClosesTermsWithEndDateBeforeCurrentDateAndStatusIs2()
        {
            // Arrange
            var mockRepository = new Mock<ITermRepository>();
            var termService = new TermService(mockRepository.Object);

            var currentDate = DateTime.Now;
            var closingDate = currentDate.AddDays(-10); // A date before the current date
            var closingTerm1 = new Term { StartDate = currentDate.AddMonths(-1).AddDays(-6), Duration = 1, Status = 2 }; // End date is before current date
            var closingTerm2 = new Term { StartDate = currentDate.AddMonths(-2), Duration = 1, Status = 2 }; // End date is before current date
            var nonClosingTerm1 = new Term { StartDate = currentDate.AddDays(-5), Duration = 1, Status = 1 }; // Active term but end date is after current date
            var nonClosingTerm2 = new Term { StartDate = currentDate.AddMonths(-3), Duration = 6, Status = 2 }; // End date is after current date

            mockRepository.Setup(repo => repo.GetAllTerms()).ReturnsAsync(new List<Term>  { closingTerm1, closingTerm2, nonClosingTerm1, nonClosingTerm2 });

            // Act
            await termService.CloseTerms();

            // Assert
            Assert.Equal(3, closingTerm1.Status); // Assert status is changed to closed
            Assert.Equal(3, closingTerm2.Status); // Assert status is changed to closed
            Assert.Equal(1, nonClosingTerm1.Status); // Assert status remains unchanged
            Assert.Equal(2, nonClosingTerm2.Status); // Assert status remains unchanged
            mockRepository.Verify(repo => repo.UpdateTerm(closingTerm1), Times.Once); // Verify UpdateTerm called for each closed term
            mockRepository.Verify(repo => repo.UpdateTerm(closingTerm2), Times.Once);
        }

        [Fact]
        public async Task CloseTerms_DoesNotCloseTermsWithEndDateAfterCurrentDate()
        {
            // Arrange
            var mockRepository = new Mock<ITermRepository>();
            var termService = new TermService(mockRepository.Object);

            var currentDate = DateTime.Now;
            var nonClosingTerm1 = new Term { StartDate = currentDate.AddMonths(-1).AddDays(2), Duration = 1, Status = 2 }; // End date is after current date
            var nonClosingTerm2 = new Term { StartDate = currentDate.AddDays(-5), Duration = 1, Status = 2 }; // End date is after current date

            mockRepository.Setup(repo => repo.GetAllTerms()).ReturnsAsync(new List<Term>  { nonClosingTerm1, nonClosingTerm2 });

            // Act
            await termService.CloseTerms();

            // Assert
            Assert.Equal(2, nonClosingTerm1.Status); // Assert status remains unchanged
            Assert.Equal(2, nonClosingTerm2.Status); // Assert status remains unchanged
            mockRepository.Verify(repo => repo.UpdateTerm(It.IsAny<Term>()), Times.Never); // Verify UpdateTerm not called
        }

        [Fact]
        public async Task CloseTerms_DoesNotCloseTermsWithStatusNotEqualTo2()
        {
            // Arrange
            var mockRepository = new Mock<ITermRepository>();
            var termService = new TermService(mockRepository.Object);

            var currentDate = DateTime.Now;
            var nonClosingTerm1 = new Term { StartDate = currentDate.AddDays(-5), Duration = 1, Status = 1 }; // Status not equal to 2
            var nonClosingTerm2 = new Term { StartDate = currentDate.AddDays(-10), Duration = 1, Status = 3 }; // Status not equal to 2

            mockRepository.Setup(repo => repo.GetAllTerms()).ReturnsAsync(new List<Term>  { nonClosingTerm1, nonClosingTerm2 });

            // Act
            await termService.CloseTerms();

            // Assert
            Assert.Equal(1, nonClosingTerm1.Status); // Assert status remains unchanged
            Assert.Equal(3, nonClosingTerm2.Status); // Assert status remains unchanged
            mockRepository.Verify(repo => repo.UpdateTerm(It.IsAny<Term>()), Times.Never); //

        }
    }
}
