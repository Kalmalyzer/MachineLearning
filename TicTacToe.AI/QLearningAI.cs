
using System;
using System.Collections.Generic;
using TicTacToe.Simulation;

namespace TicTacToe.AI
{
    public class QLearningAI
    {
        readonly float LearningRate;
        readonly float DiscountFactor;
        readonly float ExplorationEpsilon;

        Dictionary<string, float> Q = new Dictionary<string, float>();

        Random random;

        public QLearningAI(int randomSeed, float learningRate, float discountFactor, float explorationEpsilon)
        {
            random = new Random(randomSeed);
            LearningRate = learningRate;
            DiscountFactor = discountFactor;
            ExplorationEpsilon = explorationEpsilon;
        }

        private static float WinnerToReward(BoardState.Winner winner, BoardState.Player player)
        {
            switch (winner)
            {
                case BoardState.Winner.None:
                    return 0.0f;
                case BoardState.Winner.Draw:
                    return 0.5f;
                case BoardState.Winner.Player1:
                    return (player == BoardState.Player.Player1) ? 1.0f : -1.0f;
                case BoardState.Winner.Player2:
                    return (player == BoardState.Player.Player2) ? 1.0f : -1.0f;
                default:
                    throw new NotImplementedException();
            }
        }

        private static string BoardStateAndActionToQID(BoardState boardState, PlayerInput action)
        {
            Dictionary<BoardState.Player, char> boardStateToCharacter = new Dictionary<BoardState.Player, char>
            {
                { BoardState.Player.None, ' ' },
                { BoardState.Player.Player1, '1' },
                { BoardState.Player.Player2, '2' },
            };

            string s = "";
            for (int y = 0; y < boardState.Positions.GetLength(1); y++)
                for (int x = 0; x < boardState.Positions.GetLength(0); x++)
                    s += boardStateToCharacter[boardState.Positions[x, y]];

            s += action.X.ToString();
            s += action.Y.ToString();
            s += boardStateToCharacter[action.Player];

            return s;
        }

        private float ReadQ(BoardState boardState, PlayerInput action)
        {
            string qid = BoardStateAndActionToQID(boardState, action);

            if (!Q.ContainsKey(qid))
                return 0.0f;
            else
                return Q[qid];
        }

        private void WriteQ(BoardState boardState, PlayerInput action, float q)
        {
            string qid = BoardStateAndActionToQID(boardState, action);
            Q[qid] = q;
        }

        private float FindMaxQ(BoardState boardState, PlayerInput[] actions)
        {
            if (actions == null ||actions.Length == 0)
                throw new ArgumentException("Must supply at least one action");

            float q = ReadQ(boardState, actions[0]);

            for (int i = 1; i < actions.Length; i++)
                q = Math.Max(q, ReadQ(boardState, actions[i]));

            return q;
        }

        public void UpdateQ(BoardState oldBoardState, PlayerInput oldAction, BoardState newBoardState, PlayerInput[] newActions, BoardState.Winner newBoardStateWinner)
        {
            float reward = WinnerToReward(newBoardStateWinner, oldAction.Player);

            float oldQ = ReadQ(oldBoardState, oldAction);

            float maxNewQ;
            if (newBoardStateWinner != BoardState.Winner.None)
                maxNewQ = WinnerToReward(newBoardStateWinner, oldAction.Player);
            else
                maxNewQ = FindMaxQ(newBoardState, newActions);

            float updatedOldQ = oldQ + LearningRate * (reward + DiscountFactor * maxNewQ - oldQ);

            WriteQ(oldBoardState, oldAction, updatedOldQ);
        }

        public void UpdateQ(BoardState oldBoardState, PlayerInput oldAction, BoardState newBoardState)
        {
            BoardState.Winner winner = BoardState.CheckForWinner(newBoardState);

            PlayerInput[] availableNewActions;

            if (winner != BoardState.Winner.None)
                availableNewActions = null;
            else
                availableNewActions = FindAvailableActions(newBoardState, oldAction.Player).ToArray();

            UpdateQ(oldBoardState, oldAction, newBoardState, availableNewActions, winner);
        }

        private PlayerInput FindActionWithMaxQ(BoardState boardState, PlayerInput[] actions)
        {
            if (actions == null || actions.Length == 0)
                throw new ArgumentException("Must supply at least one action");

            PlayerInput bestAction = null;
            float bestQ = 0.0f;

            for (int i = 0; i < actions.Length; i++)
            {
                PlayerInput action = actions[i];
                float q = ReadQ(boardState, action);
                if (bestAction == null || q > bestQ)
                {
                    bestAction = action;
                    bestQ = q;
                }
            }

            return bestAction;
        }

        private static List<PlayerInput> FindAvailableActions(BoardState boardState, BoardState.Player player)
        {
            List<PlayerInput> availableActions = new List<PlayerInput>();

            for (int y = 0; y < boardState.Positions.GetLength(1); y++)
                for (int x = 0; x < boardState.Positions.GetLength(0); x++)
                    if (boardState.Positions[x, y] == BoardState.Player.None)
                        availableActions.Add(new PlayerInput(player, x, y));

            return availableActions;
        }

        public PlayerInput GetPlayerInput(GameState gameState, bool explorationActive)
        {
            PlayerInput[] availableActions = FindAvailableActions(gameState.BoardState, gameState.NextPlayer).ToArray();

            if (!explorationActive || (random.NextDouble() < ExplorationEpsilon))
                return FindActionWithMaxQ(gameState.BoardState, availableActions);
            else
                return availableActions[random.Next(availableActions.Length)];
        }
    }
}
