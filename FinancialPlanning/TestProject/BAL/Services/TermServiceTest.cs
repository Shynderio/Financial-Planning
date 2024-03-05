using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialPlanningBAL.Services;
using FinancialPlanningDAL.Entities;
using FinancialPlanningDAL.Repositories;
using Moq;
using Xunit;


namespace TestProject.BAL.Services
{
    public class TermServiceTests
    {
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

            var term = new Term { Status = 1 };

            mockRepository.Setup(repo => repo.UpdateTerm(It.IsAny<Term>())).Returns(Task.CompletedTask);

            // Act
            await termService.StartTerm(term);

            // Assert
            Assert.Equal(2, term.Status);
            mockRepository.Verify(repo => repo.UpdateTerm(term), Times.Once);
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
            Assert.Equal(1, term.Status);
            mockRepository.Verify(repo => repo.CreateTerm(term), Times.Once);

        }

        [Fact]
        public async Task UpdateTerm_NonExistentTerm_ShouldThrowException()
        {
            // Arrange
            var mockRepository = new Mock<ITermRepository>();
            var nonExistentTerm = new Term { Id = Guid.NewGuid(), TermName = "Non-Existent Term"};

            mockRepository.Setup(repo => repo.GetTermById(nonExistentTerm.Id))
                          .ReturnsAsync((Term)null); // Returning null as the term doesn't exist

            var termService = new TermService(mockRepository.Object);
            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => termService.UpdateTerm(nonExistentTerm));
        }

        [Fact]
        public async Task UpdateTerm_UpdatesExistingTerm()
        {
            // Arrange
            var mockRepository = new Mock<ITermRepository>();
            var termService = new TermService(mockRepository.Object);
            var termToUpdate = new Term { Id = Guid.NewGuid(), TermName = "Existing Term" };
            var updatedTerm = new Term { Id = termToUpdate.Id, TermName = "Updated Term" };

            mockRepository.Setup(repo => repo.GetTermById(termToUpdate.Id))
                          .ReturnsAsync(termToUpdate);

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
            var id = Guid.NewGuid();
            var termRepositoryMock = new Mock<ITermRepository>();
            var termToDelete = new Term { Id = id };

            termRepositoryMock.Setup(repo => repo.GetTermById(id))
                              .ReturnsAsync(termToDelete);

            var termService = new TermService(termRepositoryMock.Object);

            // Act
            await termService.DeleteTerm(id);

            // Assert
            termRepositoryMock.Verify(repo => repo.DeleteTerm(termToDelete), Times.Once);
        }

        [Fact]
        public async Task DeleteTerm_WithInvalidId_ThrowsArgumentException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var termRepositoryMock = new Mock<ITermRepository>();

            termRepositoryMock.Setup(repo => repo.GetTermById(id))
                              .ReturnsAsync((Term)null!);

            var termService = new TermService(termRepositoryMock.Object);

            // Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => termService.DeleteTerm(id));
        }
    }
}
