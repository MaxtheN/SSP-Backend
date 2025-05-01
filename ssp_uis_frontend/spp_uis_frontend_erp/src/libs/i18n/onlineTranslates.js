async function fetchData() {
    try {
      const response = await fetch('https://opensheet.elk.sh/1zfbl9REB3EUCf458GYRvFZalp8DWdWyxRSRpTHS6Yp0/1');
      if (!response.ok) {
        throw new Error('Network response was not ok');
      }
      
      const data = await response.json();
      
      return data;
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  }
  
export { fetchData };
