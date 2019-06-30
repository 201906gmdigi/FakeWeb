using System.Collections.Generic;
using System.Linq;
using CoreDAL.Dto;
using CoreDAL.Entity;
using CoreWebCommon.Dto;

namespace CoreDAL
{
    public interface IBoardDa
    {
        List<BoardDto> GetBoardData(IEnumerable<string> ids);
    }

    public class BoardDa : _BaseDA, IBoardDa
    {
        private readonly Operation _operation;

        public BoardDa(Operation operation)
        {
            _operation = operation;
        }

        public List<BoardDto> GetBoardData(IEnumerable<string> ids)
        {
            var settings = DBContext
                           .Boards
                           .Where(r => ids.Contains(r.Id))
                           .ToList()
                           .Select(MappingBoardDto)
                           .ToList();

            return settings;
        }

        /// <summary>
        /// Action Log
        /// </summary>
        /// <param name="methodName"></param>
        public bool ActionLog(string methodName)
        {
            return base.ActionLog(_operation.UserId, _operation.IP, methodName);
        }

        private static BoardDto MappingBoardDto(Board r)
        {
            return new BoardDto
            {
                Id = r.Id,
                Name = r.Name
            };
        }
    }
}