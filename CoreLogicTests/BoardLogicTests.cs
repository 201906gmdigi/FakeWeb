using System.Threading.Tasks;
using CoreDAL;
using CoreLogic;
using CoreLogic.Dto;
using CoreService.Dto;
using CoreWebCommon.Dto;
using ExpectedObjects;
using NUnit.Framework;

namespace CoreLogicTests
{
    [TestFixture]
    public class BoardLogicTests
    {
        private BoardLogicForTest _boardLogic;

        [SetUp]
        public void SetUp()
        {
            _boardLogic = new BoardLogicForTest(new Operation());
        }

        [Test]
        public async Task board_api_has_error()
        {
            GivenBoardApiResp(false);

            var boardList = await WhenGetBoardList();

            ResultShouldBe(boardList, false, "Error");
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

        private async Task<IsSuccessResult<BoardListDto>> WhenGetBoardList()
        {
            var boardList = await _boardLogic.GetBoardList(new SearchParamDto(), 10);
            return boardList;
        }

        private void GivenBoardApiResp(bool isSuccess)
        {
            _boardLogic.SetBoardQueryResp(new BoardQueryResp()
            {
                IsSuccess = isSuccess
            });
        }
    }

    internal class BoardLogicForTest : BoardLogic
    {
        private BoardQueryResp _boardQueryResp;

        public BoardLogicForTest(Operation operation, BoardDa da = null) : base(operation, da)
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