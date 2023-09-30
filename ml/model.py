import torch.nn as nn
import torch

# Definice modelu
class NeuralNet(nn.Module):
    def __init__(self):
        super(NeuralNet, self).__init__()
        self.fc1 = nn.Linear(2, 64)  # Vstupní vrstva s 2 vstupy a 64 neurony
        self.fc2 = nn.Linear(64, 32)  # Skrytá vrstva s 32 neurony
        self.fc3 = nn.Linear(32, 1)  # Výstupní vrstva s 1 výstupem

    def forward(self, x):
        x = torch.relu(self.fc1(x))
        x = torch.relu(self.fc2(x))
        x = self.fc3(x)
        return x