using LabyrinthCore.Data;
using LabyrinthCore.Graph;
using LabyrinthCore.Graph.Algorithms;

namespace LabyrinthCore.Solver;

public interface ISolver
{
     List<Vertex<Field>> GetPath(IFindPathAlgorithm algorithm, Labyrinth labyrinth);
}