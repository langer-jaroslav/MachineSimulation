import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns

# Načtení dat ze souboru DSV (např. s oddělovačem ";")
file_path = './data/data.csv'  # Nahraďte názvem vašeho souboru
data = pd.read_csv(file_path, sep=';')

data = data.drop(["Result", "DebugMinPressure", "DebugMaxPressure"], axis=1)


# Vytvoření pair plotu pro zobrazení všech možných závislostí mezi sloupci
sns.pairplot(data)
plt.suptitle('Pair plot pro všechny sloupce', y=1.02)
plt.show()
