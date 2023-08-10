using System.Collections.Generic;
using LabyrinthCore.Data;
using LabyrinthCore.Graph;

namespace LabyrinthCore.Solver;

public interface ISolver
{
     List<Vertex<Field>>? GetPath(IFindPathAlgorithm algorithm, Labyrinth labyrinth);
}