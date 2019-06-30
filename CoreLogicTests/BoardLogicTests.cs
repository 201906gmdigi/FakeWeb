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
        [Test]
        public async Task board_api_has_error()
        {
            var boardLogic = new BoardLogicForTest(new Operation());
            boardLogic.SetBoardQueryResp(new BoardQueryResp()
            {
                IsSuccess = false
            });

            var boardList = await boardLogic.GetBoardList(new SearchParamDto(), 10);

            var expected = new IsSuccessResult<BoardListDto>()
            {
                IsSuccess = false,
                ErrorMessage = "Error"
            };

            expected.ToExpectedObject().ShouldMatch(boardList);
        }
    }

    internal class BoardLogicForTest : BoardLogic
    {
        private BoardQueryResp _boardQueryResp;

        public BoardLogicForTest(Operation operation, BoardDa da = null) : base(operation, da)
        {
        }

        internal void SetBoardQueryResp(BoardQueryResp boardQueryResp)
        {
            _boardQueryResp = boardQueryResp;
        }
        protected override Task<BoardQueryResp> BoardQueryResp(BoardQueryDto queryDto)
        {
            return Task.FromResult(_boardQueryResp);
        }
    }
}