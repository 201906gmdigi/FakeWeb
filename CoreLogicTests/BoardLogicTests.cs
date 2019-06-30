using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreDAL;
using CoreDAL.Dto;
using CoreLogic;
using CoreLogic.Dto;
using CoreService.Dto;
using CoreWebCommon.Dto;
using ExpectedObjects;
using NSubstitute;
using NUnit.Framework;

namespace CoreLogicTests
{
    [TestFixture]
    public class BoardLogicTests
    {
        private IBoardDa _boardDa;
        private BoardLogicForTest _boardLogic;

        [SetUp]
        public void SetUp()
        {
            _boardDa = Substitute.For<IBoardDa>();
            _boardLogic = new BoardLogicForTest(new Operation(), _boardDa);
        }

        [Test]
        public async Task board_api_has_error()
        {
            GivenBoardApiResp(false);

            var boardList = await WhenGetBoardList();

            ResultShouldBe(boardList, false, "Error");
        }

        [Test]
        public async Task get_board_list_success_with_3_real_data_and_2_test_data()
        {
            GivenBoardApiResp(true);
            GivenBoardDataFromDb(
                new BoardDto() {Id = "11", IsTest = true},
                new BoardDto() {Id = "12", IsTest = false},
                new BoardDto() {Id = "13", IsTest = true},
                new BoardDto() {Id = "14", IsTest = false},
                new BoardDto() {Id = "16", IsTest = false});

            var boardList = await WhenGetBoardList();

            ResultShouldBeSuccess(boardList,
                                  new BoardListItem() {Id = "12"},
                                  new BoardListItem() {Id = "14"},
                                  new BoardListItem() {Id = "16"});
        }

        private static void ResultShouldBe(IsSuccessResult<BoardListDto> boardList, bool isSuccess, string errorMessage)
        {
            var expected = new IsSuccessResult<BoardListDto>()
            {
                IsSuccess = isSuccess,
                ErrorMessage = errorMessage
            };

            expected.ToExpectedObject().ShouldMatch(boardList);
        }

        private static void ResultShouldBeSuccess(IsSuccessResult<BoardListDto> boardList, params BoardListItem[] items)
        {
            var expected = new IsSuccessResult<BoardListDto>()
            {
                IsSuccess = true,
                ReturnObject = new BoardListDto()
                {
                    BoardListItems = items.ToList()
                }
            };

            expected.ToExpectedObject().ShouldMatch(boardList);
        }

        private void GivenBoardDataFromDb(params BoardDto[] dtos)
        {
            _boardDa.GetBoardData(new[] {"1"}).ReturnsForAnyArgs(dtos.ToList());
        }

        private async Task<IsSuccessResult<BoardListDto>> WhenGetBoardList()
        {
            var boardList = await _boardLogic.GetBoardList(new SearchParamDto(), 10);
            return boardList;
        }

        private void GivenBoardApiResp(bool isSuccess)
        {
            _boardLogic.SetBoardQueryResp(new BoardQueryResp()
            {
                IsSuccess = isSuccess,
                Items = new List<BoardQueryRespItem>()
            });
        }
    }

    internal class BoardLogicForTest : BoardLogic
    {
        private BoardQueryResp _boardQueryResp;

        public BoardLogicForTest(Operation operation, IBoardDa da = null) : base(operation, da)
        {
        }

        protected override Task<BoardQueryResp> BoardQueryResp(BoardQueryDto queryDto)
        {
            return Task.FromResult(_boardQueryResp);
        }

        internal void SetBoardQueryResp(BoardQueryResp boardQueryResp)
        {
            _boardQueryResp = boardQueryResp;
        }
    }
}